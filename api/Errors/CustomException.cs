using System.Net;

namespace api.Errors
{
    public class CustomException(
        string message,
        HttpStatusCode statusCode,
        Exception? innerException = null
    ) : Exception(message, innerException)
    {
        public HttpStatusCode StatusCode { get; } = statusCode;
    }
}
