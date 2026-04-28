using api.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Data.Repositories
{
    public class ProductRepository(AppDbContext context) : IProductRepository
    {
        private readonly AppDbContext _context = context;

        public async Task<ProductModel> AddProductAsync(ProductModel data)
        {
            await _context.AddAsync(data);

            var result = await _context.SaveChangesAsync();

            if (result > 0)
            {
                return data;
            }

            throw new InvalidOperationException("Failed to save product to the database");
        }

        public async Task<(List<ProductModel> products, int totalLength)> GetProductsAsync(
            int take,
            int skip,
            string? title = null
        )
        {
            var query = _context.Products.AsQueryable();

            if (!string.IsNullOrWhiteSpace(title))
            {
                query = query.Where(p => EF.Functions.Like(p.Title, $"%{title}%"));
            }

            var count = await query.CountAsync();

            var products = await query.OrderBy(p => p.Id).Skip(skip).Take(take).ToListAsync();

            return (products, count);
        }
    }
}
