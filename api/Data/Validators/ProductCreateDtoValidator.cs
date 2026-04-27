using api.Data.DTOs;
using api.Helpers;
using FluentValidation;

namespace api.Data.Validators;

public class ProductCreateDtoValidator : AbstractValidator<ProductCreateDto>
{
    public ProductCreateDtoValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage("Title is required.")
            .MaximumLength(100)
            .WithMessage("Title cannot exceed 100 characters.");

        RuleFor(x => x.Description)
            .MaximumLength(255)
            .WithMessage("Description cannot exceed 255 characters.");

        RuleFor(x => x.Image)
            .NotEmpty()
            .WithMessage("Image filename is required.")
            .Must(FileExtensions.IsValidImageExtension)
            .WithMessage("File must be a .jpeg, .jpg, .png, or .webp type.");
    }
}
