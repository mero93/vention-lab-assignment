using api.Data;
using api.Data.Models;
using api.Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace api.Tests;

public class ProductRepositoryTests : IDisposable
{
    // Actual objects
    private readonly AppDbContext _context;
    private readonly ProductRepository _repository;

    // Mock objects
    private readonly AppDbContext _mockContext;
    private readonly ProductRepository _mockRepository;

    public ProductRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new AppDbContext(options);
        _repository = new ProductRepository(_context);

        _mockContext = Substitute.ForPartsOf<AppDbContext>(options);
        _mockContext.SaveChangesAsync(Arg.Any<CancellationToken>()).Returns(0);
        _mockRepository = new ProductRepository(_mockContext);
    }

    [Fact]
    public async Task AddProductAsync_ShouldAddProductToDatabase()
    {
        var product = new ProductModel { Title = "Test", Image = "test" };

        var result = await _repository.AddProductAsync(product);

        result.Should().NotBeNull();
        result.Id.Should().BeGreaterThan(0);
        result.Title.Should().Be("Test");
        result.Image.Should().Be("test");
        _context.Products.Should().Contain(p => p.Id == result.Id);
    }

    [Fact]
    public async Task AddProductAsync_ShouldThrowInvalidOperationException()
    {
        var product = new ProductModel { Title = "Test", Image = "test" };

        await _mockRepository
            .Invoking(r => r.AddProductAsync(product))
            .Should()
            .ThrowAsync<InvalidOperationException>()
            .WithMessage("Failed to save product to the database");
    }

    [Fact]
    public async Task GetProductsAsync_ShouldReturnProductsListAndTotalLength()
    {
        var products = Enumerable
            .Range(1, 15)
            .Select(i => new ProductModel { Title = $"Product {i}", Image = $"Image {i}" })
            .ToList();

        _context.Products.AddRange(products);
        await _context.SaveChangesAsync();

        var (returnedProducts, totalCount) = await _repository.GetProductsAsync(5, 5);

        returnedProducts.Should().HaveCount(5);
        totalCount.Should().Be(15);
        returnedProducts[0].Id.Should().Be(6);
        returnedProducts[0].Title.Should().Be("Product 6");
        returnedProducts[^1].Id.Should().Be(10);
        returnedProducts[^1].Title.Should().Be("Product 10");
    }

    public void Dispose()
    {
        _context?.Dispose();
        _mockContext?.Dispose();
    }
}
