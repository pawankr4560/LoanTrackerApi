using WebApp.Model.Order;

public interface IStripeService
{
    Task<object> CreateCard(CardRequestModel model);
    Task<string> CreateCheckout(long amount, string customerId, string email, string cardId);
    Task<string> CreateCustomer(string name, string email);
    Task<string> CreateProduct(double amount);
    Task<object> DeleteCard(string id);
}