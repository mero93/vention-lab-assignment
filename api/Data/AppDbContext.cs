using System.Net.Http.Headers;
using api.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<ProductModel> Products => Set<ProductModel>();
    }
}
