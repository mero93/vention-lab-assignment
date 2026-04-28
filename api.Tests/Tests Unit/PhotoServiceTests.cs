using System.Net;
using api.Data.Services;
using api.Errors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp;

namespace api.Tests
{
    public class PhotoServiceTests : IDisposable
    {
        private readonly string _testUploadPath;
        private readonly IWebHostEnvironment _mockEnv;
        private readonly IPhotoService _service;

        public PhotoServiceTests()
        {
            _testUploadPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(_testUploadPath);
            _mockEnv = Substitute.For<IWebHostEnvironment>();
            _mockEnv.WebRootPath.Returns(_testUploadPath);
            _service = new PhotoService(_mockEnv);
        }

        [Theory]
        [InlineData("picture-1.webp")]
        [InlineData("picture-2.jpeg")]
        [InlineData("picture-3.webp")]
        [InlineData("picture-4.jpeg")]
        [InlineData("picture-5.png")]
        [InlineData("picture-6.png")]
        public async Task UploadImage_ShouldProcessVariousFormatsToWebP(string fileName)
        {
            var sourcePath = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "Resources",
                "PhotoService",
                fileName
            );

            using var fileStream = File.OpenRead(sourcePath);
            var formFile = new FormFile(fileStream, 0, fileStream.Length, "image", fileName)
            {
                Headers = new HeaderDictionary(),
                ContentType = GetContentType(fileName),
            };

            var savedName = await _service.UploadImage(formFile);
            var fullPath = Path.Combine(_testUploadPath, "uploads", savedName);

            File.Exists(fullPath).Should().BeTrue();

            Path.GetExtension(savedName).Should().Be(".webp");

            using var image = await Image.LoadAsync(fullPath);

            image.Width.Should().BeLessThanOrEqualTo(800);
            image.Height.Should().BeLessThanOrEqualTo(800);
            image
                .Metadata.DecodedImageFormat?.Name.ToLowerInvariant()
                .Should()
                .BeEquivalentTo("webp");
        }

        [Fact]
        public async Task UploadImage_ShouldCatchAnError()
        {
            var sourcePath = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "Resources",
                "PhotoService",
                "fail.webp"
            );

            using var fileStream = File.OpenRead(sourcePath);
            var formFile = new FormFile(fileStream, 0, fileStream.Length, "image", "fail.webp")
            {
                Headers = new HeaderDictionary(),
                ContentType = GetContentType("fail.webp"),
            };

            var exception = await Assert.ThrowsAsync<CustomException>(() =>
                _service.UploadImage(formFile)
            );

            exception.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            exception.Message.Should().Contain("Unknown image format");
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing && Directory.Exists(_testUploadPath))
            {
                Directory.Delete(_testUploadPath, true);
            }
        }

        ~PhotoServiceTests()
        {
            Dispose(false);
        }

        private static string GetContentType(string fileName)
        {
            var extension = Path.GetExtension(fileName).ToLowerInvariant();
            return extension switch
            {
                ".png" => "image/png",
                ".jpg" or ".jpeg" => "image/jpeg",
                ".webp" => "image/webp",
                _ => "application/octet-stream",
            };
        }
    }
}
