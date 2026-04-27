using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using api.Errors;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace api.Data.Services
{
    public class PhotoService(IWebHostEnvironment environment) : IPhotoService
    {
        private readonly IWebHostEnvironment _environment = environment;

        public async Task<string> UploadImage(IFormFile file)
        {
            var root = _environment.WebRootPath;

            var uploads = Path.Combine(root, "uploads");

            if (!Directory.Exists(uploads))
            {
                Directory.CreateDirectory(uploads);
            }

            var fileName = $"{Guid.NewGuid()}.webp";
            var fullPath = Path.Combine(uploads, fileName);

            try
            {
                using var image = await Image.LoadAsync(file.OpenReadStream());

                image.Mutate(x =>
                    x.Resize(new ResizeOptions { Size = new Size(800, 800), Mode = ResizeMode.Max })
                );

                await image.SaveAsWebpAsync(fullPath);

                return fileName;
            }
            catch (UnknownImageFormatException ex)
            {
                throw new CustomException("Unknown image format", HttpStatusCode.BadRequest);
            }
        }
    }
}
