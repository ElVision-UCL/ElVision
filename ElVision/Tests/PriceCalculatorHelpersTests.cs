using ElVision.Utilities;
using ElVisionLibrary.Models.ElPris;
using ElVisionLibrary.Models.EnergiDataService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    public class PriceCalculatorHelpersTests
    {
        [Fact]
        public void BuildConsumptionProfilePercentage_ShouldReturnDefaultProfile_WhenHourProfileIsNull()
        {
            // Arrange
            var consumptionProfile = new ConsumptionProfile { HourProfile = null };

            // Act
            var result = ProductUtilities.BuildConsumptionProfilePercentage(consumptionProfile);

            // Assert
            Assert.Equal(100.0 / 24.0 / 7.0, result[1][0]);
        }

        [Fact]
        public void GetValidProductPrice_Should_ReturnPrice_When_AnnualConsumptionIsInRange()
        {
            // Arrange
            var product = new Product
            {
                ProductPrices = new List<ProductPrice>
                {
                    new ProductPrice
                    {
                        MinimumConsumption = 1000,
                        MaximumConsumption = 5000,
                        Matrix = new List<Matrix> { new Matrix { VolumeFrom = 0, VolumeTo = 5000, Amount = 200.0 } }
                    },
                    new ProductPrice
                    {
                        MinimumConsumption = 5001,
                        MaximumConsumption = 10000,
                        Matrix = new List<Matrix> { new Matrix { VolumeFrom = 5001, VolumeTo = 10000, Amount = 300.0 } }
                    }
                }
            };
            double annualConsumption = 4000;

            // Act
            var result = ProductUtilities.GetValidProductPrice(product, annualConsumption);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200.0, result.Matrix[0].Amount);
        }

        [Fact]
        public void GetValidProductPrice_Should_ReturnNull_When_NoPriceIsInRange()
        {
            // Arrange
            var product = new Product
            {
                ProductPrices = new List<ProductPrice>
                {
                    new ProductPrice
                    {
                        MinimumConsumption = 1000,
                        MaximumConsumption = 5000,
                        Matrix = new List<Matrix> { new Matrix { VolumeFrom = 0, VolumeTo = 5000, Amount = 200.0 } }
                    }
                }
            };
            double annualConsumption = 6000;

            // Act
            var result = ProductUtilities.GetValidProductPrice(product, annualConsumption);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void CalculateAnnualProductPrice_ShouldReturnCorrectPrice_ForVariableBilling()
        {
            // Arrange
            var validProductPrice = new ProductPrice
            {
                Matrix = new List<Matrix>
                {
                    new Matrix
                    {
                        VolumeFrom = 0,
                        VolumeTo = 5000,
                        HoursFrom = 0,
                        HoursTo = 24,
                        Amount = 9
                    }
                }
            };
            var distributionAreaCharge = new DistributionAreaChargeData
            {
                Area = "DK1"
            };
            double annualConsumption = 3000;
            bool isFixedBilling = false;
            var averageDailyPrices = new List<AverageElspotPrice>
            {
                new AverageElspotPrice { TimeOfDay = TimeSpan.Parse("01:00:00"), PriceArea = "DK1", SpotPriceDKK = 5.664856703M, AmountOfElspotPricesInAverage = 30 },
                new AverageElspotPrice { TimeOfDay = TimeSpan.Parse("02:00:00"), PriceArea = "DK1", SpotPriceDKK = 5.4932613180645161290322580645M, AmountOfElspotPricesInAverage = 31 },
                new AverageElspotPrice { TimeOfDay = TimeSpan.Parse("03:00:00"), PriceArea = "DK1", SpotPriceDKK = 5.317993324M, AmountOfElspotPricesInAverage = 30 },
                new AverageElspotPrice { TimeOfDay = TimeSpan.Parse("04:00:00"), PriceArea = "DK1", SpotPriceDKK = 5.1722400346666666666666666667M, AmountOfElspotPricesInAverage = 30 },
                new AverageElspotPrice { TimeOfDay = TimeSpan.Parse("05:00:00"), PriceArea = "DK1", SpotPriceDKK = 5.5158299503333333333333333333M, AmountOfElspotPricesInAverage = 30 },
                new AverageElspotPrice { TimeOfDay = TimeSpan.Parse("06:00:00"), PriceArea = "DK1", SpotPriceDKK = 6.3523133006666666666666666667M, AmountOfElspotPricesInAverage = 30 },
                new AverageElspotPrice { TimeOfDay = TimeSpan.Parse("07:00:00"), PriceArea = "DK1", SpotPriceDKK = 8.027526707333333333333333333M, AmountOfElspotPricesInAverage = 30 },
                new AverageElspotPrice { TimeOfDay = TimeSpan.Parse("08:00:00"), PriceArea = "DK1", SpotPriceDKK = 8.529373346333333333333333333M, AmountOfElspotPricesInAverage = 30 },
                new AverageElspotPrice { TimeOfDay = TimeSpan.Parse("09:00:00"), PriceArea = "DK1", SpotPriceDKK = 7.879719978M, AmountOfElspotPricesInAverage = 30 },
                new AverageElspotPrice { TimeOfDay = TimeSpan.Parse("10:00:00"), PriceArea = "DK1", SpotPriceDKK = 7.134210051M, AmountOfElspotPricesInAverage = 30 },
                new AverageElspotPrice { TimeOfDay = TimeSpan.Parse("11:00:00"), PriceArea = "DK1", SpotPriceDKK = 6.5784032946666666666666666667M, AmountOfElspotPricesInAverage = 30 },
                new AverageElspotPrice { TimeOfDay = TimeSpan.Parse("12:00:00"), PriceArea = "DK1", SpotPriceDKK = 6.3298866713333333333333333333M, AmountOfElspotPricesInAverage = 30 },
                new AverageElspotPrice { TimeOfDay = TimeSpan.Parse("13:00:00"), PriceArea = "DK1", SpotPriceDKK = 6.3984100176666666666666666667M, AmountOfElspotPricesInAverage = 30 },
                new AverageElspotPrice { TimeOfDay = TimeSpan.Parse("14:00:00"), PriceArea = "DK1", SpotPriceDKK = 7.0272199806666666666666666667M, AmountOfElspotPricesInAverage = 30 },
                new AverageElspotPrice { TimeOfDay = TimeSpan.Parse("15:00:00"), PriceArea = "DK1", SpotPriceDKK = 8.192713331666666666666666667M, AmountOfElspotPricesInAverage = 30 },
                new AverageElspotPrice { TimeOfDay = TimeSpan.Parse("16:00:00"), PriceArea = "DK1", SpotPriceDKK = 9.975623262M, AmountOfElspotPricesInAverage = 30 },
                new AverageElspotPrice { TimeOfDay = TimeSpan.Parse("17:00:00"), PriceArea = "DK1", SpotPriceDKK = 12.488053487M, AmountOfElspotPricesInAverage = 30 },
                new AverageElspotPrice { TimeOfDay = TimeSpan.Parse("18:00:00"), PriceArea = "DK1", SpotPriceDKK = 11.056080042666666666666666667M, AmountOfElspotPricesInAverage = 30 },
                new AverageElspotPrice { TimeOfDay = TimeSpan.Parse("19:00:00"), PriceArea = "DK1", SpotPriceDKK = 9.193820046333333333333333333M, AmountOfElspotPricesInAverage = 30 },
                new AverageElspotPrice { TimeOfDay = TimeSpan.Parse("20:00:00"), PriceArea = "DK1", SpotPriceDKK = 7.3185466553333333333333333333M, AmountOfElspotPricesInAverage = 30 },
                new AverageElspotPrice { TimeOfDay = TimeSpan.Parse("21:00:00"), PriceArea = "DK1", SpotPriceDKK = 6.570709992M, AmountOfElspotPricesInAverage = 30 },
                new AverageElspotPrice { TimeOfDay = TimeSpan.Parse("22:00:00"), PriceArea = "DK1", SpotPriceDKK = 6.2253599743333333333333333333M, AmountOfElspotPricesInAverage = 30 },
                new AverageElspotPrice { TimeOfDay = TimeSpan.Parse("23:00:00"), PriceArea = "DK1", SpotPriceDKK = 5.6192333643333333333333333333M, AmountOfElspotPricesInAverage = 30 },
                new AverageElspotPrice { TimeOfDay = TimeSpan.Parse("00:00:00"), PriceArea = "DK1", SpotPriceDKK = 5.8518275989655172413793103448M, AmountOfElspotPricesInAverage = 29 }
            };

            var consumptionProfilePercentage = ProductUtilities.BuildConsumptionProfilePercentage(null);

            // Act
            var result = ProductUtilities.CalculateAnnualProductPrice(validProductPrice, annualConsumption, isFixedBilling, consumptionProfilePercentage, averageDailyPrices);

            // Assert
            Assert.Equal(27048, result, 0.0001);
        }

        [Fact]
        public void CalculatePriceFactor_ShouldReturnCorrectValue()
        {
            // Arrange
            var p1 = new ProductPriceDetails { TotalPrice = 100, TotalTax = 25 };
            var p2 = new ProductPriceDetails { TotalPrice = 200, TotalTax = 50 };
            int deliveryDays = 90;
            int restDays = 90;
            double annualConsumption = 5000;

            // Act
            var result = ProductUtilities.CalculatePriceFactor(p1, p2, deliveryDays, restDays, annualConsumption);

            // Assert
            Assert.Equal(0.0375, result, 0.0001);
        }

        [Fact]
        public void BuildConsumptionProfilePercentage_Should_ReturnCorrectPercentage_When_HourProfileIsProvided()
        {
            // Arrange
            var consumptionProfile = new ConsumptionProfile
            {
                HourProfile = new Dictionary<string, List<HourProfile>>
                {
                    { "1", new List<HourProfile> { new HourProfile { HourFrom = 0, HourTo = 6, PercentagePerHour = 10.0 } } }
                }
            };

            // Act
            var result = ProductUtilities.BuildConsumptionProfilePercentage(consumptionProfile);

            // Assert
            Assert.Equal(0.59523809523809523, result[1][0]);
        }

        [Fact]
        public void BuildConsumptionProfilePercentage_Should_ReturnEqualDistribution_When_HourProfileIsNotProvided()
        {
            // Arrange
            var consumptionProfile = new ConsumptionProfile { HourProfile = null };

            // Act
            var result = ProductUtilities.BuildConsumptionProfilePercentage(consumptionProfile);

            // Assert
            double expectedPercentage = 100.0 / 24.0 / 7.0;
            for (int day = 1; day <= 7; day++)
            {
                for (int hour = 0; hour < 24; hour++)
                {
                    Assert.Equal(expectedPercentage, result[day][hour], 2);
                }
            }
        }

        [Fact]
        public void CalculateAnnualProductPrice_Should_CalculateCorrectPrice_WithFixedBilling()
        {
            // Arrange
            var validProductPrice = new ProductPrice
            {
                Matrix = new List<Matrix>
                {
                    new Matrix
                    {
                        VolumeFrom = 0,
                        VolumeTo = 5000,
                        HoursFrom = 0,
                        HoursTo = 24,
                        Amount = 100.0
                    }
                }
            };
            double annualConsumption = 3000;
            bool isFixedBilling = true;
            var averageDailyPrices = new List<AverageElspotPrice>
            {
                new AverageElspotPrice { TimeOfDay = TimeSpan.Parse("01:00:00"), PriceArea = "DK1", SpotPriceDKK = 5.664856703M, AmountOfElspotPricesInAverage = 30 },
                new AverageElspotPrice { TimeOfDay = TimeSpan.Parse("02:00:00"), PriceArea = "DK1", SpotPriceDKK = 5.4932613180645161290322580645M, AmountOfElspotPricesInAverage = 31 },
                new AverageElspotPrice { TimeOfDay = TimeSpan.Parse("03:00:00"), PriceArea = "DK1", SpotPriceDKK = 5.317993324M, AmountOfElspotPricesInAverage = 30 },
                new AverageElspotPrice { TimeOfDay = TimeSpan.Parse("04:00:00"), PriceArea = "DK1", SpotPriceDKK = 5.1722400346666666666666666667M, AmountOfElspotPricesInAverage = 30 },
                new AverageElspotPrice { TimeOfDay = TimeSpan.Parse("05:00:00"), PriceArea = "DK1", SpotPriceDKK = 5.5158299503333333333333333333M, AmountOfElspotPricesInAverage = 30 },
                new AverageElspotPrice { TimeOfDay = TimeSpan.Parse("06:00:00"), PriceArea = "DK1", SpotPriceDKK = 6.3523133006666666666666666667M, AmountOfElspotPricesInAverage = 30 },
                new AverageElspotPrice { TimeOfDay = TimeSpan.Parse("07:00:00"), PriceArea = "DK1", SpotPriceDKK = 8.027526707333333333333333333M, AmountOfElspotPricesInAverage = 30 },
                new AverageElspotPrice { TimeOfDay = TimeSpan.Parse("08:00:00"), PriceArea = "DK1", SpotPriceDKK = 8.529373346333333333333333333M, AmountOfElspotPricesInAverage = 30 },
                new AverageElspotPrice { TimeOfDay = TimeSpan.Parse("09:00:00"), PriceArea = "DK1", SpotPriceDKK = 7.879719978M, AmountOfElspotPricesInAverage = 30 },
                new AverageElspotPrice { TimeOfDay = TimeSpan.Parse("10:00:00"), PriceArea = "DK1", SpotPriceDKK = 7.134210051M, AmountOfElspotPricesInAverage = 30 },
                new AverageElspotPrice { TimeOfDay = TimeSpan.Parse("11:00:00"), PriceArea = "DK1", SpotPriceDKK = 6.5784032946666666666666666667M, AmountOfElspotPricesInAverage = 30 },
                new AverageElspotPrice { TimeOfDay = TimeSpan.Parse("12:00:00"), PriceArea = "DK1", SpotPriceDKK = 6.3298866713333333333333333333M, AmountOfElspotPricesInAverage = 30 },
                new AverageElspotPrice { TimeOfDay = TimeSpan.Parse("13:00:00"), PriceArea = "DK1", SpotPriceDKK = 6.3984100176666666666666666667M, AmountOfElspotPricesInAverage = 30 },
                new AverageElspotPrice { TimeOfDay = TimeSpan.Parse("14:00:00"), PriceArea = "DK1", SpotPriceDKK = 7.0272199806666666666666666667M, AmountOfElspotPricesInAverage = 30 },
                new AverageElspotPrice { TimeOfDay = TimeSpan.Parse("15:00:00"), PriceArea = "DK1", SpotPriceDKK = 8.192713331666666666666666667M, AmountOfElspotPricesInAverage = 30 },
                new AverageElspotPrice { TimeOfDay = TimeSpan.Parse("16:00:00"), PriceArea = "DK1", SpotPriceDKK = 9.975623262M, AmountOfElspotPricesInAverage = 30 },
                new AverageElspotPrice { TimeOfDay = TimeSpan.Parse("17:00:00"), PriceArea = "DK1", SpotPriceDKK = 12.488053487M, AmountOfElspotPricesInAverage = 30 },
                new AverageElspotPrice { TimeOfDay = TimeSpan.Parse("18:00:00"), PriceArea = "DK1", SpotPriceDKK = 11.056080042666666666666666667M, AmountOfElspotPricesInAverage = 30 },
                new AverageElspotPrice { TimeOfDay = TimeSpan.Parse("19:00:00"), PriceArea = "DK1", SpotPriceDKK = 9.193820046333333333333333333M, AmountOfElspotPricesInAverage = 30 },
                new AverageElspotPrice { TimeOfDay = TimeSpan.Parse("20:00:00"), PriceArea = "DK1", SpotPriceDKK = 7.3185466553333333333333333333M, AmountOfElspotPricesInAverage = 30 },
                new AverageElspotPrice { TimeOfDay = TimeSpan.Parse("21:00:00"), PriceArea = "DK1", SpotPriceDKK = 6.570709992M, AmountOfElspotPricesInAverage = 30 },
                new AverageElspotPrice { TimeOfDay = TimeSpan.Parse("22:00:00"), PriceArea = "DK1", SpotPriceDKK = 6.2253599743333333333333333333M, AmountOfElspotPricesInAverage = 30 },
                new AverageElspotPrice { TimeOfDay = TimeSpan.Parse("23:00:00"), PriceArea = "DK1", SpotPriceDKK = 5.6192333643333333333333333333M, AmountOfElspotPricesInAverage = 30 },
                new AverageElspotPrice { TimeOfDay = TimeSpan.Parse("00:00:00"), PriceArea = "DK1", SpotPriceDKK = 5.8518275989655172413793103448M, AmountOfElspotPricesInAverage = 29 }
            };

            double[][] consumptionProfilePercentage = ProductUtilities.BuildConsumptionProfilePercentage(null);

            // Act
            var result = ProductUtilities.CalculateAnnualProductPrice(validProductPrice, annualConsumption, isFixedBilling, consumptionProfilePercentage, averageDailyPrices);

            // Assert
            Assert.Equal(300000.00000000006, result, 0.0001);
        }

        [Fact]
        public void CalculateNetworkFactors_Should_CalculateCorrectFactors_When_TariffsAreNotIncluded()
        {
            // Arrange
            var p1 = new ProductPriceDetails
            {
                DistributionTariff = 0.15,
                DistributionSubscription = 10,
                DistributionFee = 5,
                TransmissionTariff = 0.10,
                SystemTariff = 0.05,
                IsNetworkTariffIncluded = false
            };
            var p2 = new ProductPriceDetails
            {
                DistributionTariff = 0.20,
                DistributionSubscription = 15,
                DistributionFee = 10,
                TransmissionTariff = 0.15,
                SystemTariff = 0.10,
                IsNetworkTariffIncluded = false
            };
            int deliveryDays = 150;
            int restDays = 180;
            double annualConsumption = 10000;

            // Act
            var result = ProductUtilities.CalculateNetworkFactors(p1, p2, deliveryDays, restDays, annualConsumption);

            // Assert
            Assert.True(result.Distribution > 0);
            Assert.True(result.Subscription > 0);
        }
    }
}
