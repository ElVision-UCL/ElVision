using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElVisionLibrary.Models.ElPris
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<ZipCodeData>(myJsonResponse);
    public class DistributionArea
    {
        [Key]
        public int Id { get; set; }
        [JsonProperty(PropertyName = "gridArea")]
        public string GridArea { get; set; }
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "gridCompanyName")]
        public string GridCompanyName { get; set; }
        [JsonProperty(PropertyName = "gridCompanyLogo")]
        public string? GridCompanyLogo { get; set; }
        public List<ZipCode> ZipCodes { get; set; }
    }

    public class ZipCodeProductPriceUpdateInterval
    {
        public int id { get; set; }
        public string name { get; set; }
    }

    public class ZipCodeProductType
    {
        public int id { get; set; }
        public string name { get; set; }
    }

    public class ZipCodeData
    {
        public List<ZipCode> zipCodes { get; set; }
        public List<ZipCodeProductType> productTypes { get; set; }
        public List<ZipCodeProductPriceUpdateInterval> productPriceUpdateIntervals { get; set; }
    }

    public class ZipCode
    {
        [Key]
        public int Id { get; set; }
        [JsonProperty(PropertyName = "zipCode")]
        public int Code { get; set; }
        [JsonProperty(PropertyName = "city")]
        public string City { get; set; }
        public List<DistributionArea> DistributionAreas { get; set; }
    }
}
