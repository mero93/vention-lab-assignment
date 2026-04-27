namespace api.Data.Responses;

public class ApiResponse<T>
{
    public T? Data { get; set; }
    public object? Error { get; set; }
    public int StatusCode { get; set; }

    private ApiResponse(int statusCode, T? data, object? error)
    {
        StatusCode = statusCode;
        Data = data;
        Error = error;
    }

    public static ApiResponse<T> Success(T? data, int statusCode = 200) =>
        new(statusCode, data, null);

    public static ApiResponse<T> Failure(object error, int statusCode) =>
        new(statusCode, default, error);
}
