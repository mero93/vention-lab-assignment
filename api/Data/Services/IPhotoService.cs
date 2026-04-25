using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Data.Services
{
    public interface IPhotoService
    {
        Task<string> UploadImage(IFormFile image);
    }
}
