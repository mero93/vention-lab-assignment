namespace api.Errors
{
    public class ApiError(string message, int statusCode, string? details = null)
    {
        public string Message { get; } = message;
        public int StatusCode { get; } = statusCode;
        public string? Details { get; } = details;
    }
}
