using ElVision.Services;
using ElVisionLibrary.Models;
using ElVisionLibrary.Models.ElOverblik.TimeSeries;
using ElVisionLibrary.Models.Utilities;
using System.Globalization;

namespace ElVision.Utilities
{
    public static class TimeSeriesUtilities
    {
        public static string GetLabelForPeriod(TimeSeriesPeriod period)
        {
            return period.TimeResolution switch
            {
                "P1M" => period.PeriodEnd.ToString("MMM yyyy"),
                "PT1D" => period.PeriodStart.ToString("dd MMM"),
                "PT1H" => period.PeriodStart.ToString("HH:mm, dd MMM"),
                "P1Y" => period.PeriodStart.ToString("yyyy"),
                _ => period.PeriodStart.ToString("dd MMM")
            };
        }

        public static EnergyTimeSeries CreateEnergyTimeSeries(MyEnergyDataMarketDocument dataDocument)
        {
            return new EnergyTimeSeries
            {
                MeteringPointId = dataDocument.TimeSeries.First().MRID,
                MeasurementUnit = dataDocument.TimeSeries.First().MeasurementUnitName,
                StartDate = dataDocument.PeriodTimeInterval.Start,
                EndDate = dataDocument.PeriodTimeInterval.End,
                Periods = PopulateEnergyTimeSeriesPeriods(dataDocument.TimeSeries.First())
            };
        }

        private static List<TimeSeriesPeriod> PopulateEnergyTimeSeriesPeriods(TimeSeries timeSeries)
        {
            List<TimeSeriesPeriod> periods = new List<TimeSeriesPeriod>();
            foreach (var period in timeSeries.Period)
            {
                var periodStart = period.TimeInterval.Start;

                foreach (var point in period.Point)
                {
                    // Calculate the exact time for the point based on its position
                    var pointTime = periodStart.AddHours(int.Parse(point.Position));
                    double.TryParse(point.OutQuantityQuantity, CultureInfo.InvariantCulture, out double quantity);

                    periods.Add(new TimeSeriesPeriod
                    {
                        TimeResolution = period.Resolution,
                        PeriodStart = pointTime,
                        PeriodEnd = pointTime.AddHours(1),
                        Quantity = quantity
                    });
                }
            }
            return periods;
        }

        public static double GetUserConsumption(List<Period> periods)
        {
            return periods
                    .SelectMany(period => period.Point)
                    .Sum(point => double.Parse(
                        point.OutQuantityQuantity.ToString(),
                        CultureInfo.InvariantCulture
                    ));
        }
    }
}
