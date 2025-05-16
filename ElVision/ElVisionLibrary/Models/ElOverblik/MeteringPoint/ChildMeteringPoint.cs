using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElVisionLibrary.Models.ElOverblik.MeteringPoint
{
    public class ChildMeteringPoint
    {
        [JsonProperty("parentMeteringPointId")]
        public string ParentMeteringPointId { get; set; }

        [JsonProperty("meteringPointId")]
        public string MeteringPointId { get; set; }

        [JsonProperty("typeOfMP")]
        public string? TypeOfMP { get; set; }

        [JsonProperty("meterReadingOccurrence")]
        public string? MeterReadingOccurrence { get; set; }

        [JsonProperty("meterNumber")]
        public string? MeterNumber { get; set; }
    }
}
