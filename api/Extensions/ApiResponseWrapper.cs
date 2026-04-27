using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
                    || objectResult.Value is ApiResponse<object>
                )
                {
                    return;
                }

                objectResult.Value = ApiResponse<object>.Success(objectResult.Value, statusCode);
            }
        }

        public void OnResultExecuted(ResultExecutedContext context) { }
    }
}
