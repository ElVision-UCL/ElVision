using ElVision.Utilities;
using ElVisionLibrary.Models.EnergiDataService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    public class GetDailyAverageTests
    {
        [Fact]
        public void GetDailyAverage_ShouldReturnCorrectAverages()
        {
            // Arrange
            var elspotPrices = new List<ElspotPrice>
            {
                new ElspotPrice { HourDK = DateTime.Parse("2024-11-25T08:00:00"), PriceArea = "DK1", SpotPriceDKK = 100 },
                new ElspotPrice { HourDK = DateTime.Parse("2024-11-25T08:00:00"), PriceArea = "DK1", SpotPriceDKK = 200 },
                new ElspotPrice { HourDK = DateTime.Parse("2024-11-25T08:00:00"), PriceArea = "DK2", SpotPriceDKK = 300 },
                new ElspotPrice { HourDK = DateTime.Parse("2024-11-25T09:00:00"), PriceArea = "DK1", SpotPriceDKK = 400 },
            };

            // Act
            var result = ElspotUtilities.GetDailyAverage(elspotPrices);

            // Assert
            Assert.Equal(3, result.Count);

            var dk1_8am = result.Single(p => p.PriceArea == "DK1" && p.TimeOfDay == TimeSpan.FromHours(8));
            Assert.Equal(150, dk1_8am.SpotPriceDKK);
            Assert.Equal(2, dk1_8am.AmountOfElspotPricesInAverage);

            var dk2_8am = result.Single(p => p.PriceArea == "DK2" && p.TimeOfDay == TimeSpan.FromHours(8));
            Assert.Equal(300, dk2_8am.SpotPriceDKK);
            Assert.Equal(1, dk2_8am.AmountOfElspotPricesInAverage);

            var dk1_9am = result.Single(p => p.PriceArea == "DK1" && p.TimeOfDay == TimeSpan.FromHours(9));
            Assert.Equal(400, dk1_9am.SpotPriceDKK);
            Assert.Equal(1, dk1_9am.AmountOfElspotPricesInAverage);
        }

        [Fact]
        public void GetDailyAverage_ShouldReturnEmptyList_WhenNoElspotPrices()
        {
            // Arrange
            var elspotPrices = new List<ElspotPrice>();

            // Act
            var result = ElspotUtilities.GetDailyAverage(elspotPrices);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void GetDailyAverage_ShouldHandleSingleElspotPrice()
        {
            // Arrange
            var elspotPrices = new List<ElspotPrice>
            {
                new ElspotPrice { HourDK = DateTime.Parse("2024-11-25T08:00:00"), PriceArea = "DK1", SpotPriceDKK = 100 },
            };

            // Act
            var result = ElspotUtilities.GetDailyAverage(elspotPrices);

            // Assert
            Assert.Single(result);

            var singleResult = result.First();
            Assert.Equal("DK1", singleResult.PriceArea);
            Assert.Equal(TimeSpan.FromHours(8), singleResult.TimeOfDay);
            Assert.Equal(100, singleResult.SpotPriceDKK);
            Assert.Equal(1, singleResult.AmountOfElspotPricesInAverage);
        }
    }
}
