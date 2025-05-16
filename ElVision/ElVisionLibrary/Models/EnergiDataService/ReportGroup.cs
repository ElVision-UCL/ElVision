namespace ElVisionLibrary.Models.EnergiDataService
{
    public class ReportGroup
    {
        public string ReportGrp { get; set; }     // Name of the group (Solar, Hydro, etc.)
        public string ReportGrpCode { get; set; }  // Code for the group (R01, R02, etc.)
        public double TotalSharePpm { get; set; }  // Total Share PPM for this group
        public double UserShareKwh { get; set; }   // The calculated user's share in kWh
    }
}