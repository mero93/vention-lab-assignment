using api.Data.DTOs;
using api.Data.Models;
using api.Data.Repositories;
using AutoMapper;

namespace api.Data.Services
{
    public class ProductService(IProductRepository repository, IMapper mapper) : IProductService
    {
        private readonly IProductRepository _repository = repository;
        private readonly IMapper _mapper = mapper;

        public async Task<ProductDto> CreateProductAsync(ProductCreateDto data)
        {
            var result = await _repository.AddProductAsync(_mapper.Map<ProductModel>(data));

            return _mapper.Map<ProductDto>(result);
        }

        public async Task<PaginatedResult<ProductDto>> GetPaginatedProductsAsync(
            QueryParams parameters
        )
        {
            var (list, totalLength) = await _repository.GetProductsAsync(
                parameters.Take,
                parameters.Skip,
                parameters.Title
            );

            return new PaginatedResult<ProductDto>(
                _mapper.Map<List<ProductDto>>(list),
                totalLength,
                parameters.PageNumber,
                parameters.PageSize
            );
        }
    }
}
