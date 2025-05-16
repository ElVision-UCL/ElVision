using ElVision.Utilities;
using ElVisionLibrary.Models.EnergiDataService;

namespace Tests
{
    public class CalculateClimateReportTests
    {
        [Theory]
        [InlineData("R01-1", "R01")]
        [InlineData("R01-2", "R01")]
        [InlineData("R01-3", "R01")]
        [InlineData("R05-1", "R02")]
        [InlineData("R05-2", "R02")]
        [InlineData("R03", "R03")]
        public void NormalizeReportGroupCode_ShouldReturnExpectedCode(string inputCode, string expectedCode)
        {
            // Act
            var normalizedCode = ClimateUtilities.NormalizeReportGroupCode(inputCode);

            // Assert
            Assert.Equal(expectedCode, normalizedCode);
        }

        [Fact]
        public void GroupClimateRecords_ShouldGroupRecordsCorrectly()
        {
            // Arrange
            var climateRecords = new List<ClimateRecord>
            {
                new ClimateRecord { ReportGrpCode = "R01-1", SharePPM = 100 },
                new ClimateRecord { ReportGrpCode = "R01-2", SharePPM = 200 },
                new ClimateRecord { ReportGrpCode = "R03", SharePPM = 300 },
            };

            // Act
            var groupedRecords = ClimateUtilities.GroupClimateRecords(climateRecords);

            // Assert
            Assert.Equal(2, groupedRecords.Count);
            Assert.Equal(300, groupedRecords["R01"].TotalSharePpm);
            Assert.Equal(300, groupedRecords["R03"].TotalSharePpm);
        }

        [Fact]
        public void CalculateReport_ShouldReturnCorrectUserShares()
        {
            // Arrange
            var climateRecords = new List<ClimateRecord>
            {
                new ClimateRecord { ReportGrpCode = "R01-1", SharePPM = 100 },
                new ClimateRecord { ReportGrpCode = "R01-2", SharePPM = 200 },
                new ClimateRecord { ReportGrpCode = "R03", SharePPM = 300 },
            };
            double userConsumptionKwh = 600;

            // Act
            var report = ClimateUtilities.CalculateReport(climateRecords, userConsumptionKwh);

            // Assert
            var r01Group = report.ReportGroups.Single(r => r.ReportGrpCode == "R01");
            var r03Group = report.ReportGroups.Single(r => r.ReportGrpCode == "R03");

            Assert.Equal(300, r01Group.UserShareKwh);
            Assert.Equal(300, r03Group.UserShareKwh);
        }
    }
}