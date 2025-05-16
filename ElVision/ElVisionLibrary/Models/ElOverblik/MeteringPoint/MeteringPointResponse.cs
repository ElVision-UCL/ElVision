using Newtonsoft.Json;

namespace ElVisionLibrary.Models.ElOverblik.MeteringPoint
{
    public class MeteringPointResponse
    {
        [JsonProperty("result")]
        public List<MeteringPointModel> MeteringPoints { get; set; }
    }
}
