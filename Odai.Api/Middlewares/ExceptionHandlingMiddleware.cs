using System.Net;
using System.Text.Json;

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
            var logDirectory = Path.Combine(Directory.GetCurrentDirectory(), "logs");
            if (!Directory.Exists(logDirectory))
            {
                Directory.CreateDirectory(logDirectory);
            }
            var logFileName = $"errors-{DateTime.Now:yyyy-MM-dd}.txt";
            var logPath = Path.Combine(logDirectory, logFileName);
            var errorLog = $@"
                ====================
                Date:{DateTime.Now}
                Path:{context.Request.Path}
                Message:{ex.Message}
                StackTrace:{ex.StackTrace}
                ====================";

            await File.AppendAllTextAsync(logPath, errorLog);

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
