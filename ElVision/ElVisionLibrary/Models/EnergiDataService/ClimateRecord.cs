using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElVisionLibrary.Models.EnergiDataService
{
    public class ClimateRecord
    {
        [JsonProperty("HourUTC")]
        public DateTime HourUTC { get; set; }

        [JsonProperty("HourDK")]
        public DateTime HourDK { get; set; }

        [JsonProperty("PriceArea")]
        public string PriceArea { get; set; }

        [JsonProperty("ConnectedArea")]
        public string ConnectedArea { get; set; }

        [JsonProperty("ReportGrpCode")]
        public string ReportGrpCode { get; set; }

        [JsonProperty("ReportGrp")]
        public string ReportGrp { get; set; }

        [JsonProperty("SharePPM")]
        public int SharePPM { get; set; }

        [JsonProperty("ShareMWh")]
        public double ShareMWh { get; set; }

        [JsonProperty("CO2Emission")]
        public double CO2Emission { get; set; }
    }
}
