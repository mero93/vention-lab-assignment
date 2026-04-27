using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using api.Data;
using api.Data.DTOs;
using api.Data.Models;
using api.Data.Responses;
using api.Errors;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace api.Tests
{
    public class ProductsControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private readonly WebApplicationFactory<Program> _factory;
        private readonly SqliteConnection _connection;

        public ProductsControllerTests(WebApplicationFactory<Program> factory)
        {
            _connection = new SqliteConnection("DataSource=:memory:");
            _connection.Open();

            _factory = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    var sqlite = services.SingleOrDefault(d =>
                        d.ServiceType == typeof(DbContextOptions<AppDbContext>)
                    );

                    if (sqlite != null)
                        services.Remove(sqlite);

                    services.AddDbContext<AppDbContext>(options =>
                    {
                        options.UseSqlite(_connection);
                    });

                    var serviceProvider = services.BuildServiceProvider();
                    using var scope = serviceProvider.CreateScope();

                    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                    db.Database.EnsureCreated();

                    var products = Enumerable
                        .Range(1, 15)
                        .Select(id => new ProductModel
                        {
                            Title = $"Product-{id}",
                            Image = "test image",
                        })
                        .ToList();

                    db.Products.AddRange(products);

                    db.SaveChanges();
                });
            });

            _client = _factory.CreateClient();
        }

        [Theory]
        [InlineData(1, 5, 5)]
        [InlineData(2, 10, 5)]
        [InlineData(99, 5, 0)]
        public async Task GetProducts_ReturnsWrappedSuccess_WithPagination(
            int page,
            int size,
            int expectedCount
        )
        {
            var response = await _client.GetAsync(
                $"/api/products?pageNumber={page}&pageSize={size}"
            );

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var content = await response.Content.ReadFromJsonAsync<
                ApiResponse<PaginatedResult<ProductDto>>
            >();
            content.Should().NotBeNull();
            content.Data?.Items.Should().HaveCount(expectedCount);
            content.Error.Should().BeNull();
        }

        [Fact]
        public async Task CreateProduct_ReturnsWrappedSuccess_WithObject()
        {
            var newProduct = new ProductCreateDto
            {
                Title = "Test title",
                Description = "Test Description",
                Image = "turntable.webp",
            };

            var response = await _client.PostAsJsonAsync("/api/products", newProduct);

            response.StatusCode.Should().Be(HttpStatusCode.Created);

            var result = await response.Content.ReadFromJsonAsync<ApiResponse<ProductDto>>();
            result?.Data.Should().NotBeNull();
            result?.Data?.Title.Should().Be("Test title");
            result?.Error.Should().BeNull();
        }

        [Fact]
        public async Task CreateProduct_ReturnsWrappedFail_FailsValidation()
        {
            var invalidProduct = new ProductCreateDto
            {
                Title = "Invalid Product",
                Description = "Testing validation",
                Image = "malware.exe",
            };

            var response = await _client.PostAsJsonAsync("/api/products", invalidProduct);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var result = await response.Content.ReadFromJsonAsync<ApiResponse<ProductDto>>();

            result!.Error.Should().NotBeNull();
            result
                .Error.Message.Should()
                .Contain("File must be a .jpeg, .jpg, .png, or .webp type.");
        }

        [Fact]
        public async Task CreateProduct_ReturnsWrappedFail_GeneralException()
        {
            var response = await _client.PostAsync(
                "/api/products",
                new StringContent("{{", Encoding.UTF8, "application/json")
            );

            response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);

            var result = await response.Content.ReadFromJsonAsync<ApiResponse<object>>();

            result!.Error.Should().NotBeNull();
            result!.Error!.Message.Should().Be("An unexpected server error occurred.");
        }
    }
}
