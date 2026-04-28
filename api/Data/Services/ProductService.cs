using api.Data.DTOs;
using api.Data.Models;
using api.Data.Repositories;
using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;

namespace api.Data.Services
{
    public class ProductService(IProductRepository repository, IMapper mapper, IMemoryCache cache)
        : IProductService
    {
        private readonly IProductRepository _repository = repository;
        private readonly IMapper _mapper = mapper;
        private readonly IMemoryCache _cache = cache;
        private readonly string _cacheKeyPrefix = "ProductList_";
        private static CancellationTokenSource _resetCacheToken = new();

        public async Task<ProductDto> CreateProductAsync(ProductCreateDto data)
        {
            var result = await _repository.AddProductAsync(_mapper.Map<ProductModel>(data));

            if (result != null)
            {
                var oldToken = Interlocked.Exchange(
                    ref _resetCacheToken,
                    new CancellationTokenSource()
                );
                await oldToken.CancelAsync();
                oldToken.Dispose();
            }

            return _mapper.Map<ProductDto>(result);
        }

        public async Task<PaginatedResult<ProductDto>> GetPaginatedProductsAsync(
            QueryParams parameters
        )
        {
            string cacheKey =
                $"{_cacheKeyPrefix}_page:{parameters.PageNumber}_size:{parameters.PageSize}_title:{parameters.Title}";

            if (
                _cache.TryGetValue(cacheKey, out PaginatedResult<ProductDto>? cachedResult)
                && cachedResult != null
            )
            {
                return cachedResult;
            }

            var (list, totalLength) = await _repository.GetProductsAsync(
                parameters.Take,
                parameters.Skip,
                parameters.Title
            );

            var result = new PaginatedResult<ProductDto>(
                _mapper.Map<List<ProductDto>>(list),
                totalLength,
                parameters.PageNumber,
                parameters.PageSize
            );

            var cacheOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromHours(1))
                .AddExpirationToken(new CancellationChangeToken(_resetCacheToken.Token));

            _cache.Set(cacheKey, result, cacheOptions);

            return result;
        }
    }
}
