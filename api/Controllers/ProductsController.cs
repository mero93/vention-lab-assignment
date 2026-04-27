using api.Data.DTOs;
using api.Data.Services;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController(IProductService productService) : ControllerBase
    {
        private readonly IProductService _productService = productService;

        [HttpGet]
        public async Task<ActionResult> GetProducts([FromQuery] PaginationParams paginationParams)
        {
            var result = await _productService.GetPaginatedProductsAsync(paginationParams);

            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult> CreateProduct(
            [FromBody] ProductCreateDto dto,
            [FromServices] IValidator<ProductCreateDto> validator
        )
        {
            var validationResult = await validator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors[0].ErrorMessage);
            }

            var product = await _productService.CreateProductAsync(dto);
            return StatusCode(201, product);
        }
    }
}
