using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

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
