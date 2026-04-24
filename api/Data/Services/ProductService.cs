using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data.DTOs;
using api.Data.Repositories;
using AutoMapper;

namespace api.Data.Services
{
    public class ProductService(IProductRepository repository, IMapper mapper) : IProductService
    {
        private readonly IProductRepository _repository = repository;
        private readonly IMapper _mapper = mapper;

        public Task<ProductDto> CreateProductASync(ProductCreateDto data)
        {
            throw new NotImplementedException();
        }

        public Task<PaginatedResult<ProductDto>> GetPaginatedProductsASync(
            PaginationParams paginationParams
        )
        {
            throw new NotImplementedException();
        }
    }
}
