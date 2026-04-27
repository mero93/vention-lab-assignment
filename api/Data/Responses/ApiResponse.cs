using api.Errors;

namespace api.Data.Responses;

public class ApiResponse<T>(int statusCode, T? data, ApiError? error)
{
    public T? Data { get; set; } = data;
    public ApiError? Error { get; set; } = error;
    public int StatusCode { get; set; } = statusCode;
}
