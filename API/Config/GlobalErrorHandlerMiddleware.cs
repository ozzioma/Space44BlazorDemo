using System;
using System.Threading.Tasks;
using Common;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace API.Config
{
    public class GlobalErrorHandlerMiddleware
    {
        private readonly RequestDelegate next;
        private ILogger<GlobalErrorHandlerMiddleware> logger;

        public GlobalErrorHandlerMiddleware(RequestDelegate next, ILogger<GlobalErrorHandlerMiddleware> _logger)
        {
            this.next = next;
            logger = _logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
                logger.LogError(ex.Message, ex);
            }
        }


        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var problemDetails = new CommandResult<string>
            {
                ErrorFlag = true,
                Response = exception.Message,
                Title = "Error processing request",
                Message = exception.Message,
                Detail = exception.InnerException?.Message,
                Status = StatusCodes.Status500InternalServerError,
                //ValidationErrors = errors,
            };

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            return context.Response.WriteAsync(problemDetails.ToJson());
        }
    }
}