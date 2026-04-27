using api.Helpers;
using FluentValidation;

namespace api.Data.Validators;

public class PhotoUploadValidator : AbstractValidator<IFormFile>
{
    private readonly long _maxFileSize = 5 * 1024 * 1024;

    public PhotoUploadValidator()
    {
        RuleFor(x => x.Length)
            .NotNull()
            .LessThanOrEqualTo(_maxFileSize)
            .WithMessage("File size must be less than 5MB.");

        RuleFor(x => x.FileName)
            .Must(FileExtensions.IsValidImageExtension)
            .WithMessage("File must be a .jpeg, .jpg, .png, or .webp type.");
    }
}
