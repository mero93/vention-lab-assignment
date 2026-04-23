using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data.Models;

namespace api.Data.Repositories
{
    public class ProductRepository(AppDbContext database) : IProductRepository
    {
        public Task<ProductModel> AddProductAsync(ProductModel data)
        {
            throw new NotImplementedException();
        }

        public Task<(List<ProductModel> products, int totalLength)> GetProductsAsync(
            int take,
            int skip
        )
        {
            throw new NotImplementedException();
        }
    }
}
