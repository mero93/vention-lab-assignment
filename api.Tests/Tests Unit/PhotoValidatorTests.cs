using api.Data.Validators;
using FluentValidation.TestHelper;
using Microsoft.AspNetCore.Http;

namespace api.Tests;

public class PhotoValidatorTests
{
    private readonly PhotoUploadValidator _validator = new();

    [Fact]
    public void Should_Have_Error_When_File_Is_Too_Large()
    {
        var mockFile = Substitute.For<IFormFile>();

        mockFile.Length.Returns(6 * 1024 * 1024);
        mockFile.FileName.Returns("large_image.jpg");

        var result = _validator.TestValidate(mockFile);
        result.ShouldHaveValidationErrorFor(x => x.Length);
    }

    [Fact]
    public void Should_Have_Error_When_Image_Has_Invalid_Extension()
    {
        var mockFile = Substitute.For<IFormFile>();

        mockFile.Length.Returns(1 * 1024 * 1024);
        mockFile.FileName.Returns("wrong_extension.exe");

        var result = _validator.TestValidate(mockFile);
        result
            .ShouldHaveValidationErrorFor(p => p.FileName)
            .WithErrorMessage("File must be a .jpeg, .jpg, .png, or .webp type.");
    }
}
