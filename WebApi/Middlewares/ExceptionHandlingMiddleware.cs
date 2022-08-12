using System.Net;
using System.Text.Json;
using WebApi.RequestResponseModels;

namespace WebApi.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;
        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            var response = context.Response;

            var errorResponse = new ErrorResponse
            {
                Success = false
            };

            switch (exception)
            {
                case StarbuxValidationException ex:
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    errorResponse.Message = ex.Message;
                    break;
                case ApplicationException ex:
                    if (ex.Message.Contains("Invalid token"))
                    {
                        response.StatusCode = (int)HttpStatusCode.Forbidden;
                        errorResponse.Message = ex.Message;
                        break;
                    }
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    errorResponse.Message = ex.Message;
                    break;
                case KeyNotFoundException ex:
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    errorResponse.Message = ex.Message;
                    break;
                default:
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    errorResponse.Message = "Internal Server errors. Check Logs!";
                    break;
            }
            _logger.LogError(exception, exception.Message);
            errorResponse.StatusCode = response.StatusCode;
            var result = JsonSerializer.Serialize(errorResponse);
            await context.Response.WriteAsync(result);
        }


        // private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        // {

        //     if (exception is Helpers.Exceptions.StarbuxValidationException || exception is SecurityTokenException)
        //     {
        //         var response = context.Response;
        //         response.ContentType = "application/json";
        //         response.StatusCode = (int)HttpStatusCode.BadRequest;
        //         var result = JsonSerializer.Serialize(new ErrorResponse
        //         {
        //             Success = false,
        //             StatusCode = response.StatusCode,
        //             Message = exception.Message
        //         });
        //         await response.WriteAsync(result);
        //     }
        //     else
        //     {
        //         throw exception;
        //     }
        // }
    }
}