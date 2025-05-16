using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElVisionLibrary.Models.EnergiDataService
{
    public class ClimateResponse
    {
        [JsonProperty("records")]
        public List<ClimateRecord> ClimateRecords { get; set; }
    }
}
