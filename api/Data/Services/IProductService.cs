using api.Data.DTOs;

namespace api.Data.Services
{
    public interface IProductService
    {
        Task<ProductDto> CreateProductAsync(ProductCreateDto data);
        Task<PaginatedResult<ProductDto>> GetPaginatedProductsAsync(QueryParams parameters);
    }
}
