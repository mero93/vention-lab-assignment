using api.Data.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace api.Extensions
{
    public class ApiResponseWrapper : IResultFilter
    {
        public void OnResultExecuting(ResultExecutingContext context)
        {
            if (context.Result is ObjectResult objectResult)
            {
                int statusCode = objectResult.StatusCode ?? StatusCodes.Status200OK;

                if (
                    statusCode >= 400
                    || statusCode == StatusCodes.Status204NoContent
                    || objectResult.Value is null
                    || (
                        objectResult.Value.GetType().IsGenericType
                        && objectResult.Value.GetType().GetGenericTypeDefinition()
                            == typeof(ApiResponse<>)
                    )
                )
                {
                    return;
                }

                var valueType = objectResult.Value.GetType();

                var genericType = typeof(ApiResponse<>).MakeGenericType(valueType);

                var wrappedResult = Activator.CreateInstance(
                    genericType,
                    statusCode,
                    objectResult.Value,
                    null
                );

                objectResult.Value = wrappedResult;
            }
        }

        public void OnResultExecuted(ResultExecutedContext context) { }
    }
}
