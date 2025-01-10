using Stripe;
using WebApp.Service.Auth;

namespace WebApp.Service.Stripe
{
    public class StripeService : IStripeService
    {
        private readonly IAuthService _authService;
        private readonly global::Stripe.CustomerService _customerService;

        public StripeService(IAuthService authService,
            CustomerService customerService)
        {
            _authService = authService;
            _customerService = customerService;
        }

        public async Task<dynamic> CreateCustomer(string name , string email)
        {
            try
            {
                return await _customerService.CreateAsync(new CustomerCreateOptions
                {
                    Name = name,
                    Email = email,
                    Address = new AddressOptions
                    {
                        Line1 =  "443",
                        Line2 = "Sector 43",
                        City = "Mohali",
                        State = "Punjab",
                        PostalCode = "140601",
                        Country = "India"
                    }
                });
            }
            catch (Exception) { throw; }
        }

        public async Task<dynamic> CreateProduct()
        {
            try
            {
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
