using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElVisionLibrary.Models.EnergiDataService
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Elspot>(myJsonResponse);
    public class ElspotPrice
    {
        [JsonProperty("HourUTC")]
        public DateTime HourUTC { get; set; }
        [JsonProperty("HourDK")]
        public DateTime HourDK { get; set; }
        [JsonProperty("PriceArea")]
        public string PriceArea { get; set; }
        [JsonProperty("SpotPriceDKK")]
        public decimal SpotPriceDKK { get; set; }
        [JsonProperty("SpotPriceEUR")]
        public decimal SpotPriceEUR { get; set; }
    }

    public class AverageElspotPrice
    {
        public TimeSpan TimeOfDay { get; set; }
        public string PriceArea { get; set; }
        public decimal SpotPriceDKK { get; set; }
        public int AmountOfElspotPricesInAverage { get; set; }
    }

    public class ElspotResponse
    {
        [JsonProperty("total")]
        public int Total { get; set; }
        [JsonProperty("limit")]
        public int Limit { get; set; }
        [JsonProperty("dataset")]
        public string Dataset { get; set; }
        [JsonProperty("records")]
        public List<ElspotPrice> ElspotPrices { get; set; }

        public void GetToday()
        {
            var today = DateTime.Today;
            List<ElspotPrice> todayPrices = new List<ElspotPrice>();
            foreach (ElspotPrice price in ElspotPrices)
            {
                if (today.Month == price.HourDK.Month && today.Day == price.HourDK.Day)
                {
                    todayPrices.Add(price);
                }
            }

            ElspotPrices = todayPrices;
        }

        public void AddMoms()
        {
            foreach (ElspotPrice price in ElspotPrices)
            {
                price.SpotPriceDKK *= 1.25m;
            }
        }

        public void GetLastMonthOnly()
        {
            var now = DateTime.Now;
            var lastMonth = now.AddMonths(-1);
            List<ElspotPrice> lastMonthPrices = new List<ElspotPrice>();
            foreach(var elspotPrice in ElspotPrices)
            {
                if(elspotPrice.HourDK.Month == lastMonth.Month)
                {
                    lastMonthPrices.Add(elspotPrice);
                }
            }

            ElspotPrices = lastMonthPrices.OrderBy(x => x.HourDK).ToList();
        }
        public void ConvertMwhToKwh()
        {
            for (int i = 0; i < ElspotPrices.Count; i++)
            {
                ElspotPrices[i].SpotPriceDKK /= 1000;
                ElspotPrices[i].SpotPriceEUR /= 1000;
            }
        }

        public void RoundPricesToTwoDecimals()
        {
            for(int i = 0;i < ElspotPrices.Count; i++)
            {
                ElspotPrices[i].SpotPriceDKK = Math.Round(ElspotPrices[i].SpotPriceDKK, 2, MidpointRounding.AwayFromZero);
            }
        }

        public void GetPricesWithTariffs(List<NationalTariff> nationalTariffs, List<ChargeOwnerTariffForHour> chargeOwnerTariffs)
        {
            for (int i = 0; i < ElspotPrices.Count; i++)
            {
                ElspotPrices[i].SpotPriceDKK = ElspotPrices[i].SpotPriceDKK + (decimal)nationalTariffs[0].Price + (decimal)nationalTariffs[1].Price + (decimal)nationalTariffs[2].Price + (decimal)chargeOwnerTariffs[i].Price;
            }
        }

        public void ConvertKronerToØre()
        {
            for (int i = 0; i < ElspotPrices.Count; i++)
            {
                ElspotPrices[i].SpotPriceDKK *= 100;
                ElspotPrices[i].SpotPriceEUR *= 100;
            }
        }
    }
}
