using System.Reflection;
using System.Text.Json.Serialization;

namespace ElVisionLibrary.Models.Utilities
{
    public class ModelStateErrors
    {
        [JsonPropertyName("errors")]
        public Dictionary<string, List<string>>? Errors { get; set; }
    }
}
