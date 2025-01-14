using Microsoft.AspNetCore.Http;
using Stripe;
using WebApp.Data.Entity;
using WebApp.Data.Repository;
using WebApp.Model.Constant;
using WebApp.Service.Auth;
using WebApp.Service.Stripe;

public class StripeService : IStripeService
{
    private readonly IAuthService _authService;
    private readonly CustomerService _customerService;
    private readonly IGenericRepository<StripeCustomer> _customerRepo;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly Stripe.ProductService _productService;
    private string userId = string.Empty;
    private readonly PriceService _priceService;

    public StripeService(
        IAuthService authService,
        CustomerService customerService,
        IGenericRepository<StripeCustomer> customerRepo,
        IHttpContextAccessor httpContextAccessor,
        Stripe.ProductService productService,
        PriceService priceService)
    {
        _authService = authService;
        _customerService = customerService;
        _customerRepo = customerRepo;
        _httpContextAccessor = httpContextAccessor;
        _productService = productService;
        var claims = _httpContextAccessor.HttpContext.User.Claims.ToList();
        userId = claims.FirstOrDefault(x=> x.Type == "Id").Value;
        _priceService = priceService;
    }

    public async Task<string> CreateCustomer(string name, string email)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name cannot be null or empty.", nameof(name));
        if (string.IsNullOrWhiteSpace(email)) throw new ArgumentException("Email cannot be null or empty.", nameof(email));

        if (string.IsNullOrEmpty(userId))
            throw new InvalidOperationException("User ID not found in claims.");
        try
        {
            var customerCreateOptions = new CustomerCreateOptions
            {
                Name = name,
                Email = email,
                Address = new AddressOptions
                {
                    Line1 = "443",
                    Line2 = "Sector 43",
                    City = "Mohali",
                    State = "Punjab",
                    PostalCode = "140601",
                    Country = "India"
                }
            };

            var result = await _customerService.CreateAsync(customerCreateOptions);
            var customer = new StripeCustomer
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                IsActive = true,
                IsDeleted = false,
                CustomerId = result.Id,
                CreatedOn = DateTime.UtcNow,
            };

            _customerRepo.Insert(customer);
            _customerRepo.Save();

            return result.Id;
        }
        catch (Exception ex)
        {
            throw new Exception("Error creating customer.", ex);
        }
    }

    public async Task<string> CreateProduct(double amount)
    {
        try
        {
            var product = await _productService.CreateAsync(new ProductCreateOptions
            {
                Name = ConstantVarriable.ProductName,
            });

            var price = await _priceService.CreateAsync(new PriceCreateOptions
            {
                UnitAmount = (long)(amount * 100), 
                Currency = "inr",
                Product = product.Id
            });

            return price.Id;
        }
        catch (Exception) { throw; }
    }
}
