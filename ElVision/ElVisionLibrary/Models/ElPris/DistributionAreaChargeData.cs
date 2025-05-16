using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ElVisionLibrary.Models.ElPris
{
    public class DistributionAreaChargeData
    {
        [JsonPropertyName("distributionAreaCharges")]
        public List<DistributionAreaCharge> DistributionAreaCharges { get; set; }

        [JsonPropertyName("distributionAreaGridArea")]
        public string DistributionAreaGridArea { get; set; }

        [JsonPropertyName("area")]
        public string Area { get; set; }
    }

    public class DistributionAreaCharge
    {
        [JsonPropertyName("chargeId")]
        public string ChargeId { get; set; }

        [JsonPropertyName("validFrom")]
        public DateTime ValidFrom { get; set; }

        [JsonPropertyName("validTo")]
        public DateTime ValidTo { get; set; }

        [JsonPropertyName("chargeType")]
        public string ChargeType { get; set; }

        [JsonPropertyName("billingType")]
        public string BillingType { get; set; }

        [JsonPropertyName("price")]
        public double Price { get; set; }

        [JsonPropertyName("distributionAreaChargeHours")]
        public List<DistributionAreaChargeHour> DistributionAreaChargeHours { get; set; }
    }

    public class DistributionAreaChargeHour
    {
        [JsonPropertyName("hourFrom")]
        public int HoursFrom { get; set; }

        [JsonPropertyName("hourTo")]
        public int HoursTo { get; set; }

        [JsonPropertyName("amount")]
        public double Amount { get; set; }
    }
}
