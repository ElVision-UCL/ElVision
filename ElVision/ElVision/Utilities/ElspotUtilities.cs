using ElVisionLibrary.Models.EnergiDataService;

namespace ElVision.Utilities
{
    public static class ElspotUtilities
    {
        public static List<AverageElspotPrice> GetDailyAverage(List<ElspotPrice> elspotPrices)
        {
            return elspotPrices
                .GroupBy(price => new { price.HourDK.TimeOfDay, price.PriceArea })
                .Select(group => new AverageElspotPrice
                {
                    TimeOfDay = group.Key.TimeOfDay,
                    PriceArea = group.Key.PriceArea,
                    SpotPriceDKK = group.Average(price => price.SpotPriceDKK),
                    AmountOfElspotPricesInAverage = group.Count()
                })
                .ToList();
        }
    }
}
