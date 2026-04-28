using api.Data.DTOs;
using api.Data.Models;
using api.Data.Repositories;
using api.Data.Services;
using AutoMapper;
using NSubstitute.ExceptionExtensions;

namespace api.Tests
{
    public class ProductServiceTests
    {
        private readonly IProductRepository _repo;
        private readonly IMapper _mapper;
        private readonly ProductService _service;

        public ProductServiceTests()
        {
            _repo = Substitute.For<IProductRepository>();
            _mapper = Substitute.For<IMapper>();

            _service = new ProductService(_repo, _mapper);
        }

        [Fact]
        public async Task CreateProductAsync_ShouldInvokeInCorrectOrderAndReturnData()
        {
            var dto = new ProductCreateDto { Title = "Product", Image = "test.jpg" };
            var model = new ProductModel { Title = "Product", Image = "test.jpg" };
            var savedModel = new ProductModel
            {
                Id = 1,
                Title = "Product",
                Image = "test.jpg",
            };
            var expectedDto = new ProductDto
            {
                Id = 1,
                Title = "Product",
                Image = "test.jpg",
            };

            _mapper.Map<ProductModel>(dto).Returns(model);
            _repo.AddProductAsync(model).Returns(savedModel);
            _mapper.Map<ProductDto>(savedModel).Returns(expectedDto);

            var result = await _service.CreateProductAsync(dto);

            Received.InOrder(() =>
            {
                _mapper.Map<ProductModel>(dto);
                _repo.AddProductAsync(model);
                _mapper.Map<ProductDto>(savedModel);
            });

            result.Should().NotBeNull();
            result.Id.Should().Be(1);
            result.Title.Should().Be(dto.Title);
        }

        [Fact]
        public async Task CreateProductAsync_ShouldThrow_WhenRepoFails()
        {
            var dto = new ProductCreateDto { Title = "Error", Image = "test.jpg" };
            _mapper
                .Map<ProductModel>(dto)
                .Returns(new ProductModel { Title = "Error", Image = "test.jpg" });
            _repo.AddProductAsync(Arg.Any<ProductModel>()).Throws(new InvalidOperationException());

            await _service
                .Invoking(s => s.CreateProductAsync(dto))
                .Should()
                .ThrowAsync<InvalidOperationException>();
        }

        [Fact]
        public async Task GetProductsAsync_ShouldReturnCorrectPaginationMetadata()
        {
            var paginationParams = new PaginationParams { PageNumber = 2, PageSize = 10 };
            var products = new List<ProductModel>();
            var dtos = new List<ProductDto>();

            _repo
                .GetProductsAsync(paginationParams.Take, paginationParams.Skip)
                .Returns((products, 25));
            _mapper.Map<List<ProductDto>>(products).Returns(dtos);

            var result = await _service.GetPaginatedProductsAsync(paginationParams);

            Assert.Equal(25, result.TotalCount);
            Assert.Equal(3, result.TotalPages);
            Assert.Equal(2, result.PageNumber);
            Assert.NotNull(result.Items);
            await _repo.Received(1).GetProductsAsync(10, 10);
        }
    }
}
