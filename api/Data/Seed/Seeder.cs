using api.Data.Models;
using Bogus;

namespace api.Data.Seed;

public static class Seeder
{
    public static void Seed(AppDbContext context)
    {
        if (context.Products.Any())
            return;

        var stockImages = Enumerable.Range(1, 6).Select(id => $"stock-{id}.webp").ToList();

        var productFaker = new Faker<ProductModel>()
            .RuleFor(p => p.Title, f => f.Commerce.ProductName())
            .RuleFor(
                p => p.Description,
                f =>
                    f.Commerce.ProductDescription().Length <= 255
                        ? f.Commerce.ProductDescription()
                        : f.Lorem.Sentence(8)
            )
            .RuleFor(p => p.Image, f => f.PickRandom(stockImages));

        var fakeProducts = productFaker.Generate(50);

        context.Products.AddRange(fakeProducts);
        context.SaveChanges();
    }
}
