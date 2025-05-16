using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElVisionLibrary.Models.ElOverblik.MeteringPoint
{
    public class MeteringPointModel
    {
        [JsonProperty("meteringPointId")]
        public string MeteringPointId { get; set; }

        [JsonProperty("typeOfMP")]
        public string TypeOfMP { get; set; }

        [JsonProperty("balanceSupplierName")]
        public string EnergySupplierName { get; set; }

        [JsonProperty("postcode")]
        public string Postcode { get; set; }

        [JsonProperty("cityName")]
        public string CityName { get; set; }

        [JsonProperty("hasRelation")]
        public bool HasRelation { get; set; }

        [JsonProperty("consumerCVR")]
        public string ConsumerCVR { get; set; }

        [JsonProperty("dataAccessCVR")]
        public string DataAccessCVR { get; set; }

        [JsonProperty("childMeteringPoints")]
        public List<ChildMeteringPoint>? ChildMeteringPoints { get; set; }

        [JsonProperty("streetCode")]
        public string StreetCode { get; set; }

        [JsonProperty("streetName")]
        public string StreetName { get; set; }

        [JsonProperty("buildingNumber")]
        public string BuildingNumber { get; set; }

        [JsonProperty("floorId")]
        public string FloorId { get; set; }

        [JsonProperty("roomId")]
        public string RoomId { get; set; }

        [JsonProperty("citySubDivisionName")]
        public string CitySubDivisionName { get; set; }

        [JsonProperty("municipalityCode")]
        public string MunicipalityCode { get; set; }

        [JsonProperty("locationDescription")]
        public string LocationDescription { get; set; }

        [JsonProperty("settlementMethod")]
        public string SettlementMethod { get; set; }

        [JsonProperty("meterReadingOccurrence")]
        public string MeterReadingOccurrence { get; set; }

        [JsonProperty("firstConsumerPartyName")]
        public string FirstConsumerPartyName { get; set; }

        [JsonProperty("secondConsumerPartyName")]
        public string SecondConsumerPartyName { get; set; }

        [JsonProperty("meterNumber")]
        public string MeterNumber { get; set; }

        [JsonProperty("consumerStartDate")]
        public DateTime ConsumerStartDate { get; set; }
        public string PriceArea { get; set; }

        public void SetPriceArea()
        {
            var postCode = int.Parse(Postcode);
            if (postCode >= 5000)
            {
                PriceArea = "DK1";
            }
            else
            {
                PriceArea = "DK2";
            }
        }
    }
}
