using Domin.Exceptions;
using Shared;

namespace Api.Middleware
{
    public class GlobalErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalErrorHandlingMiddleware> _logger;

        public GlobalErrorHandlingMiddleware(RequestDelegate request, ILogger<GlobalErrorHandlingMiddleware> logger)
        {
            _next = request;
            _logger = logger;
        }


        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                // Call the next middleware in the pipeline
                await _next.Invoke(context);
                //if After calling the next middleware, the response status code is 404
                //           it means the endpoint was not found
                await EndPointNotFound(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                //Handle the exception
                //1- set status code for Response
                //2- set content type for Response
                //3- set response body
                //4- return the respons

                context.Response.ContentType = "application/json";

                var Response = new ErrorDetails()
                {
                    Message = ex.Message,
                };
                var satusCode = ex switch
                {
                    //Handle Not Found Exception
                    NotFoundException => StatusCodes.Status404NotFound,
                    BadResquestException => StatusCodes.Status400BadRequest,
                    ValidationErrorsExecption => ValidationErrors((ValidationErrorsExecption)ex, Response),
                    _ => StatusCodes.Status500InternalServerError,
                };
                context.Response.StatusCode = satusCode; 
                Response.Status = satusCode;
                await context.Response.WriteAsJsonAsync(Response);
            }

        }

        private static async Task EndPointNotFound(HttpContext context)
        {
            if (context.Response.StatusCode == 404)
            {
                context.Response.ContentType = "application/json";
                var response = new ErrorDetails()
                {
                    Status = StatusCodes.Status404NotFound,
                    Message = $"this {context.Request.Path} Not Found", //,mead this End point is not found
                };
                await context.Response.WriteAsJsonAsync(response);
            }
        }
        private static int ValidationErrors(ValidationErrorsExecption ex, ErrorDetails errorDetails)
        {
            errorDetails.Errors = ex.Errors;
            return StatusCodes.Status400BadRequest;
        }
    }
}
