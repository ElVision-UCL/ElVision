using ElVisionLibrary.Models.EnergiDataService;

namespace ElVision.Utilities
{
    public static class ClimateUtilities
    {
        public static ClimateReport CalculateReport(List<ClimateRecord> climateRecords, double userConsumptionKwh)
        {
            // Group records by report group codes
            var groupedRecords = GroupClimateRecords(climateRecords);

            // Calculate total shares
            double totalSharePpm = groupedRecords.Sum(g => g.Value.TotalSharePpm);

            // Calculate user share for each report group
            CalculateUserShares(groupedRecords, totalSharePpm, userConsumptionKwh);

            // Build and return the climate report
            return BuildClimateReport(groupedRecords);
        }

        public static Dictionary<string, ReportGroup> GroupClimateRecords(List<ClimateRecord> climateRecords)
        {
            var reportGroups = new Dictionary<string, ReportGroup>();

            foreach (var record in climateRecords)
            {
                string reportGrpCode = NormalizeReportGroupCode(record.ReportGrpCode);

                if (!reportGroups.ContainsKey(reportGrpCode))
                {
                    reportGroups[reportGrpCode] = new ReportGroup
                    {
                        ReportGrp = record.ReportGrp,
                        ReportGrpCode = reportGrpCode,
                        TotalSharePpm = 0,
                        UserShareKwh = 0
                    };
                }

                reportGroups[reportGrpCode].TotalSharePpm += record.SharePPM;
            }

            return reportGroups;
        }

        public static string NormalizeReportGroupCode(string reportGrpCode)
        {
            if (reportGrpCode == "R01-1" || reportGrpCode == "R01-2" || reportGrpCode == "R01-3")
            {
                return "R01";
            }

            if (reportGrpCode.StartsWith("R05"))
            {
                return "R02";
            }

            return reportGrpCode;
        }

        private static void CalculateUserShares(Dictionary<string, ReportGroup> reportGroups, double totalSharePpm, double userConsumptionKwh)
        {
            foreach (var reportGroup in reportGroups.Values)
            {
                double shareRatio = reportGroup.TotalSharePpm / totalSharePpm;
                reportGroup.UserShareKwh = userConsumptionKwh * shareRatio;
            }
        }

        private static ClimateReport BuildClimateReport(Dictionary<string, ReportGroup> reportGroups)
        {
            return new ClimateReport
            {
                ReportGroups = reportGroups.Values.ToList()
            };
        }
    }
}
