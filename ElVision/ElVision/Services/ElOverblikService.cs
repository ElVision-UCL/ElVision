using ElVision.Utilities;
using ElVisionLibrary.Models.ElOverblik;
using ElVisionLibrary.Models.ElOverblik.MeteringPoint;
using ElVisionLibrary.Models.ElOverblik.TimeSeries;
using ElVisionLibrary.Models.Utilities;
using Newtonsoft.Json;
using System.Text;

namespace ElVision.Services
{
    public interface IElOverblikService
    {
        Task<OperationResult> GetMeteringPointAsync(string dataAccessToken);
        Task<OperationResult> GetDataAccessTokenAsync(string userToken);
        Task<OperationResult> GetTimeSeriesAsync(string dataAccessToken, string meteringPointId, DateTime startDate, DateTime endDate, string period);
    }
    public class ElOverblikService(HttpClient httpClient, ILogger<ElOverblikService> logger) : IElOverblikService
    {
        private readonly HttpClient httpClient = httpClient;
        private readonly ILogger<ElOverblikService> logger = logger;

        public async Task<OperationResult> GetMeteringPointAsync(string dataAccessToken)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, "https://api.eloverblik.dk/customerapi/api/meteringpoints/meteringpoints?includeAll=false");
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", dataAccessToken);
                var response = await httpClient.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                {
                    return await ResponseHandlers.Handle_Unsuccessful_ResponseAsync(response);
                }

                return await ResponseHandlers.Handle_Successful_ResponseAsync<MeteringPointResponse>(response);
            }
            catch (HttpRequestException ex)
            {
                logger.LogError(ex, "Network error occurred while fetching the metering points.");
                return ResponseHandlers.Handle_NetworkErrorResponse(ex);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An unexpected error occurred.");
                return ResponseHandlers.Handle_UnexpectedResponse(ex);
            }
        }

        public async Task<OperationResult> GetDataAccessTokenAsync(string userToken)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, "https://api.eloverblik.dk/customerapi/api/token");
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", userToken);
                var response = await httpClient.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                {
                    return await ResponseHandlers.Handle_Unsuccessful_ResponseAsync(response);
                }

                return await ResponseHandlers.Handle_Successful_ResponseAsync<TokenResponse>(response);
            }
            catch (HttpRequestException ex)
            {
                logger.LogError(ex, "Network error occurred while fetching the case.");
                return ResponseHandlers.Handle_NetworkErrorResponse(ex);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An unexpected error occurred.");
                return ResponseHandlers.Handle_UnexpectedResponse(ex);
            }
        }

        public async Task<OperationResult> GetTimeSeriesAsync(string dataAccessToken, string meteringPointId, DateTime startDate, DateTime endDate, string period)
        {
            try
            {
                string url = $"https://api.eloverblik.dk/customerapi/api/meterdata/gettimeseries/{startDate.ToString("yyyy-MM-dd")}/{endDate.ToString("yyyy-MM-dd")}/{period}";
                var request = new HttpRequestMessage(HttpMethod.Post, url);
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", dataAccessToken);
                
                var body = new
                {
                    meteringPoints = new
                    {
                        meteringPoint = new[] { meteringPointId }
                    }
                };

                var jsonBody = JsonConvert.SerializeObject(body);
                request.Content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

                var response = await httpClient.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                {
                    return await ResponseHandlers.Handle_Unsuccessful_ResponseAsync(response);
                }

                return await ResponseHandlers.Handle_Successful_ResponseAsync<TimeSeriesResponse>(response);
            }
            catch (HttpRequestException ex)
            {
                logger.LogError(ex, "Network error occurred while fetching the case.");
                return ResponseHandlers.Handle_NetworkErrorResponse(ex);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An unexpected error occurred.");
                return ResponseHandlers.Handle_UnexpectedResponse(ex);
            }
        }
    }
}
