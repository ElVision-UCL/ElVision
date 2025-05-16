using ElVision.Utilities;
using ElVisionLibrary.Models;
using ElVisionLibrary.Models.ElOverblik.MeteringPoint;
using ElVisionLibrary.Models.ElOverblik.TimeSeries;
using ElVisionLibrary.Models.EnergiDataService;
using ElVisionLibrary.Models.Utilities;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Reflection.Metadata;

namespace ElVision.Services
{
    public interface IClimateReportService
    {
        Task<Dictionary<string, List<ClimateRecord>>> GetClimateRecordsAsync();
        Task GetClimateReportAsync();
    }
    public class ClimateReportService : IClimateReportService
    {
        private static readonly HttpClient _httpClient = new HttpClient();
        private const string Url = "https://api.energidataservice.dk/dataset/DeclarationTransmissionGridmix";
        private readonly ILogger<ClimateReportService> logger;
        private Dictionary<string, List<ClimateRecord>> ClimateRecords { get; set; } = new Dictionary<string, List<ClimateRecord>>();

        public ClimateReportService(ILogger<ClimateReportService> logger)
        {
            this.logger = logger;
        }

        public async Task<Dictionary<string, List<ClimateRecord>>> GetClimateRecordsAsync()
        {
            if (ClimateRecords == null || !ClimateRecords.Any())
            {
                await GetClimateReportAsync();
            }

            return ClimateRecords;
        }

        public async Task GetClimateReportAsync()
        {
            try
            {
                // Set end date as today and start date as one year before
                var endDate = DateTime.UtcNow;
                var startDate = endDate.AddYears(-1);

                // Create a dictionary to hold the climate records for different price areas
                var climateReports = new Dictionary<string, List<ClimateRecord>>();

                // List of price areas to loop through
                var priceAreas = new[] { "DK1", "DK2" };

                foreach (var priceArea in priceAreas)
                {
                    // Create the query string for each price area
                    var queryString = string.Join("&", new[]
                    {
                        $"offset=0",
                        $"start={WebUtility.UrlEncode(startDate.ToString("yyyy-MM-ddTHH:mm", CultureInfo.InvariantCulture))}",
                        $"end={WebUtility.UrlEncode(endDate.ToString("yyyy-MM-ddTHH:mm", CultureInfo.InvariantCulture))}",
                        $"filter={WebUtility.UrlEncode($"{{\"PriceArea\":[\"{priceArea}\"]}}")}",
                        $"sort={WebUtility.UrlEncode("HourUTC DESC")}"
                    });

                    var requestUri = $"{Url}?{queryString}";

                    var response = await _httpClient.GetAsync(requestUri);
                    if (response.IsSuccessStatusCode)
                    {
                        var json = await response.Content.ReadAsStringAsync();
                        var data = JsonConvert.DeserializeObject<ClimateResponse>(json);
                        climateReports[priceArea] = data.ClimateRecords;
                    }
                    else
                    {
                        logger.LogError($"Getting climate report failed. status code: {response.StatusCode} - {response.ReasonPhrase}");
                    }
                    // Assign the result to the ClimateRecords property
                    ClimateRecords = climateReports;
                }
            }
            catch (HttpRequestException ex)
            {
                logger.LogError(ex, "Network error occurred while fetching the metering points.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An unexpected error occurred.");
            }
        }

    }
}
