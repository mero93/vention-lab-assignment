using System.Net;
using api.Errors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace api.Tests
{
    public class GlobalExceptionHandlerTests
    {
        private readonly GlobalExceptionHandler _middleware;
        private readonly IWebHostEnvironment _mockEnv;
        private readonly ILogger<GlobalExceptionHandler> _mockLogger;

        public GlobalExceptionHandlerTests()
        {
            _mockLogger = Substitute.For<ILogger<GlobalExceptionHandler>>();
            _mockEnv = Substitute.For<IWebHostEnvironment>();
            _middleware = new GlobalExceptionHandler(_mockLogger, _mockEnv);

            _mockEnv.EnvironmentName.Returns("Development");
        }

        [Fact]
        public async Task GlobalExceptionHandler_ShouldCatchCustomException()
        {
            var context = new DefaultHttpContext();
            context.Response.Body = new MemoryStream();

            var customException = new CustomException(
                "Validation failed",
                HttpStatusCode.BadRequest
            );

            RequestDelegate next = innerContext => throw customException;

            await _middleware.InvokeAsync(context, next);

            context.Response.StatusCode.Should().Be(400);
        }

        [Fact]
        public async Task GlobalExceptionHandler_ShouldCatchGeneralException()
        {
            var context = new DefaultHttpContext();
            context.Response.Body = new MemoryStream();

            var exception = new Exception("General exception");

            RequestDelegate next = innerContext => throw exception;

            await _middleware.InvokeAsync(context, next);

            context.Response.StatusCode.Should().Be(500);
        }
    }
}
