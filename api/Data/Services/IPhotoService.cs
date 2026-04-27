namespace api.Data.Services
{
    public interface IPhotoService
    {
        Task<string> UploadImage(IFormFile file);
    }
}
