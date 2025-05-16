using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElVisionLibrary.Models.EnergiDataService
{
    public class ClimateReport
    {
        public List<ReportGroup> ReportGroups { get; set; }

        public void RoundPricesToTwoDecimals()
        {
            for (int i = 0; i < ReportGroups.Count; i++)
            {
                ReportGroups[i].UserShareKwh = Math.Round(ReportGroups[i].UserShareKwh, 2, MidpointRounding.AwayFromZero);
            }
        }
    }
}
