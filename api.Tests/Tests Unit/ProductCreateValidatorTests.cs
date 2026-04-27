using api.Data.DTOs;
using api.Data.Validators;
using FluentValidation.TestHelper;

namespace api.Tests;

public class ProductCreateDtoValidatorTests
{
    private readonly ProductCreateDtoValidator _validator = new();

    [Fact]
    public void Should_Have_Error_When_Title_Is_Empty()
    {
        var model = new ProductCreateDto { Title = "", Image = "test.jpg" };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(p => p.Title);
    }

    [Fact]
    public void Should_Have_Error_When_Image_Has_Invalid_Extension()
    {
        var model = new ProductCreateDto { Title = "Valid Title", Image = "virus.exe" };
        var result = _validator.TestValidate(model);
        result
            .ShouldHaveValidationErrorFor(p => p.Image)
            .WithErrorMessage("File must be a .jpeg, .jpg, .png, or .webp type.");
    }

    [Fact]
    public void Should_Not_Have_Error_When_Model_Is_Valid()
    {
        var model = new ProductCreateDto
        {
            Title = "Proper Title",
            Description = "Short desc",
            Image = "photo.webp",
        };
        var result = _validator.TestValidate(model);
        result.ShouldNotHaveAnyValidationErrors();
    }
}
