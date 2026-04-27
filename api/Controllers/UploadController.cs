using api.Data.Services;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UploadController(IPhotoService photoService) : ControllerBase
{
    private readonly IPhotoService _photoService = photoService;

    [HttpPost]
    public async Task<ActionResult> UploadImage(
        IFormFile file,
        [FromServices] IValidator<IFormFile> validator
    )
    {
        if (file == null)
        {
            throw new FluentValidation.ValidationException("No file uploaded.");
        }

        var validationResult = await validator.ValidateAsync(file);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors[0].ErrorMessage);
        }

        var fileName = await _photoService.UploadImage(file);

        return Ok(new { FileName = fileName });
    }
}
