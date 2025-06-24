
namespace Odai.Shared.Auth
{
    public class Response<T>
    {
        public bool Succeeded { get; set; }
        public string Message { get; set; }
        public List<string> Errors { get; set; }
        public T Data { get; set; }
        public int StatusCode { get; set; }

        public Response(T data, int statusCode, string? message = null)
        {
            Succeeded = true;
            Message = message ?? "Operation succeeded";
            Data = data;
            StatusCode = statusCode;
        }
        public Response(bool succeeded = true)
        {
            Succeeded = succeeded;
            Message = "SUCCEED";
        }
        public Response(bool succeeded, int statusCode, string? message = null)
        {
            Succeeded = succeeded;
            StatusCode = statusCode;
            Message = message ?? (succeeded ? "SUCCEED" : "FAILED");
        }
        public Response(string message, int statusCode)
        {
            Succeeded = false;
            Message = message;
            Errors = new List<string> { message };
            StatusCode = statusCode;
        }
        public Response(List<string> errors, int statusCode)
        {
            Succeeded = false;
            Errors = errors;
            Message = "One or more errors occurred.";
            StatusCode = statusCode;
        }
    }
}
