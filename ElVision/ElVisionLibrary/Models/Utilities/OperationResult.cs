using System.Net;

namespace ElVisionLibrary.Models.Utilities
{
    public class OperationResult
    {
        public bool Success { get; set; }
        public Dictionary<string, List<string>>? ModelStateErrors { get; set; }
        public ProblemDetails? ProblemDetails { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;
    }

    public class OperationResult<T> : OperationResult
    {
        public T? Data { get; set; }
    }
}


