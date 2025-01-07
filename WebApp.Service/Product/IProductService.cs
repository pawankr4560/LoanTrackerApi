using WebApp.Model.Product;

namespace WebApp.Service.Product
{
    public interface IProductService
    {
        Task<bool> Add(CreateProductRequestModel model);
        Task<bool> Delete(Guid id);
        IEnumerable<Data.Entity.Product> ProductList();
        Task<bool> Update(UpdateProductModel model);
    }
}