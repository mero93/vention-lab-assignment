using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data.DTOs;

namespace api.Data.Services
{
    public interface IProductService
    {
        Task<ProductDto> CreateProductASync(ProductCreateDto data);
        Task<PaginatedResult<ProductDto>> GetPaginatedProductsASync(
            PaginationParams paginationParams
        );
    }
}
