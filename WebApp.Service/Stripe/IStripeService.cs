
namespace WebApp.Service.Stripe
{
    public interface IStripeService
    {
        Task<string> CreateCustomer(string name, string email);
        Task<string> CreateProduct(double amount);
    }
}