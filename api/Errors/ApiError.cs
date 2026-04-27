namespace api.Errors
{
    public class ApiError(string message, int statusCode, string? details = null)
    {
        string Message { get; } = message;
        int StatusCode { get; } = statusCode;
        string? Details { get; } = details;
    }
}
