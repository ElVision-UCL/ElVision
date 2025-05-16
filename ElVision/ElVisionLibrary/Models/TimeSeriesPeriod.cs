using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElVisionLibrary.Models
{
    public class TimeSeriesPeriod
    {
        public string TimeResolution { get; set; } // Time resolution, fx P1M for 1 month, P1D for 1 day.
        public DateTime PeriodStart { get; set; }
        public DateTime PeriodEnd { get; set; }
        public double Quantity { get; set; } // The read value for the given period.
    }
}
