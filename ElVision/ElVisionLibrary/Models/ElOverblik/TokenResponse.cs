using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElVisionLibrary.Models.ElOverblik
{
    public class TokenResponse
    {
        [JsonProperty("result")]
        public string Result { get; set; }
    }
}
