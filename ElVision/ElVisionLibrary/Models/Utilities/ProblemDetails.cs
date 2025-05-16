using System.Text.Json.Serialization;

namespace ElVisionLibrary.Models.Utilities
{
    public class ProblemDetails
    {
        [JsonPropertyName("instance")]
        public string? Instance { get; set; }
        [JsonPropertyName("title")]
        public string? Title { get; set; }
        [JsonPropertyName("statusCode")]
        public int StatusCode { get; set; }
        [JsonPropertyName("detail")]
        public string? Detail { get; set; }
    }
}
