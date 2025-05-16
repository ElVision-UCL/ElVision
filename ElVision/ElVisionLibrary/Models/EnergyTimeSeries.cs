using ElVisionLibrary.Models.ElOverblik.TimeSeries;
using ElVisionLibrary.Models.EnergiDataService;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElVisionLibrary.Models
{
    public class EnergyTimeSeries
    {
        public string MeteringPointId { get; set; } // Unique Metering point ID
        public string MeasurementUnit { get; set; } // Measurement unit, e.g., KWH
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<TimeSeriesPeriod> Periods { get; set; } // Periods within the time series

        public void RoundToTwoDecimals()
        {
            for (int i = 0; i < Periods.Count; i++)
            {
                Periods[i].Quantity = Math.Round(Periods[i].Quantity, 2, MidpointRounding.AwayFromZero);
            }
        }


    }
}
