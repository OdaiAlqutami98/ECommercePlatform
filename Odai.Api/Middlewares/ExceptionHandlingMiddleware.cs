using System.Net;
using System.Text.Json;
using Serilog;

namespace ECommercePlatform.Api.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {

                await HandleExceptionAsync(context, ex);
            }
        }
        private async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            Log.Error(ex, "An error occurred while processing the request. Path: {Path}", context.Request.Path);
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";

            var response = new
            {
                Succeeded = false,
                Message = "Something went wrong. Please try again later.",
                Errors = new[] { ex.Message }
            };

            var result = JsonSerializer.Serialize(response);
            await context.Response.WriteAsync(result);

        }
    }
}
