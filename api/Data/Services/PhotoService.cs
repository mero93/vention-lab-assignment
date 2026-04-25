using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Data.Services
{
    public class PhotoService(IWebHostEnvironment environment) : IPhotoService
    {
        private readonly IWebHostEnvironment _environment = environment;

        public Task<string> UploadImage(IFormFile image)
        {
            throw new NotImplementedException();
        }
    }
}
