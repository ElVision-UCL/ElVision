using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace ElVisionLibrary.Models.ElPris
{
    public class StandardConsumptionProfilesData
    {
        [JsonProperty("STANDARD_CONSUMPTION_PROFILES")]
        public StandardConsumptionProfiles StandardConsumptionProfiles { get; set; }
    }
    public class StandardConsumptionProfiles
    {
        [JsonProperty("private")]
        public PrivateConsumption Private { get; set; }
    }

    public class PrivateConsumption
    {
        [JsonProperty("profiles")]
        public List<ConsumptionProfile> Profiles { get; set; }
    }

    public class ConsumptionProfile
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("profileNo")]
        public int ProfileNo { get; set; }
        [JsonProperty("hourProfile")]
        public Dictionary<string, List<HourProfile>> HourProfile { get; set; }
    }

    public class HourProfile
    {
        [JsonProperty("hourFrom")]
        public int HourFrom { get; set; }
        [JsonProperty("hourTo")]
        public int HourTo { get; set; }
        [JsonProperty("percentagePerHour")]
        public double PercentagePerHour { get; set; }
    }
}
