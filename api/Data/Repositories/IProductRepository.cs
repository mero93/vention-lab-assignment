using api.Data.Models;

namespace api.Data.Repositories
{
    public interface IProductRepository
    {
        Task<ProductModel> AddProductAsync(ProductModel data);

        Task<(List<ProductModel> products, int totalLength)> GetProductsAsync(int take, int skip);
    }
}
