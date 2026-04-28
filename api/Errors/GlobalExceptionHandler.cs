using System.Net;
using System.Text.Json;
using api.Data.Responses;

namespace api.Errors
{
    public class GlobalExceptionHandler(
        ILogger<GlobalExceptionHandler> logger,
        IHostEnvironment environment
    ) : IMiddleware
    {
        private readonly ILogger _logger = logger;
        private readonly IHostEnvironment _environment = environment;
        private readonly JsonSerializerOptions _jsonOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            var (statusCode, message) = exception switch
            {
                CustomException customException => (
                    (int)customException.StatusCode,
                    customException.Message
                ),
                FluentValidation.ValidationException validationException => (
                    (int)HttpStatusCode.BadRequest,
                    validationException.Message
                ),
                _ => (
                    (int)HttpStatusCode.InternalServerError,
                    "An unexpected server error occurred."
                ),
            };

            _logger.LogError(exception, "Unhandled Exception: {Message}", exception.Message);

            context.Response.StatusCode = statusCode;

            var errorResponse = new ApiError(
                message,
                statusCode,
                _environment.IsDevelopment() ? exception.ToString() : null
            );

            var response = new ApiResponse<object>(statusCode, null, errorResponse);

            await context.Response.WriteAsync(JsonSerializer.Serialize(response, _jsonOptions));
        }
    }
}
