using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ElVisionLibrary.Models.ElPris
{
    public class NordpoolData
    {
        [JsonPropertyName("nordpools")]
        public List<Nordpool> Nordpools { get; set; }
    }

    public class Nordpool
    {
        [JsonPropertyName("area")]
        public string Area { get; set; }

        [JsonPropertyName("hourProcent")]
        public List<double> HourProcent { get; set; }
    }
}
