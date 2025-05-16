using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ElVisionLibrary.Models.ElPris
{
    public class NationalChargesData
    {
        [JsonPropertyName("nationalCharges")]
        public List<NationalCharge> NationalCharges { get; set; }
    }

    public class NationalCharge
    {
        [JsonPropertyName("chargeType")]
        public string ChargeType { get; set; }

        [JsonPropertyName("chargeId")]
        public string ChargeId { get; set; }

        [JsonPropertyName("chargeDescription")]
        public string ChargeDescription { get; set; }

        [JsonPropertyName("amount")]
        public double Amount { get; set; }
    }
}
