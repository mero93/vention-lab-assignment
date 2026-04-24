using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public async Task<ProductDto> CreateProductASync(ProductCreateDto data)
        {
            var result = await _repository.AddProductAsync(_mapper.Map<ProductModel>(data));

            return _mapper.Map<ProductDto>(result);
        }

        public async Task<PaginatedResult<ProductDto>> GetPaginatedProductsASync(
            PaginationParams paginationParams
        )
        {
            var (list, totalLength) = await _repository.GetProductsAsync(
                paginationParams.Take,
                paginationParams.Skip
            );

            return new PaginatedResult<ProductDto>(
                _mapper.Map<List<ProductDto>>(list),
                totalLength,
                paginationParams.PageNumber,
                paginationParams.PageSize
            );
        }
    }
}
