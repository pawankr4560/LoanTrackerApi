
namespace WebApp.Service.Stripe
{
    public interface IStripeService
    {
        Task<dynamic> CreateProduct();
        Task<dynamic> CreateCustomer(string name, string email);
    }
}