using ElVision.Utilities;
using ElVisionLibrary.Models.EnergiDataService;
using ElVisionLibrary.Models.Utilities;

namespace ElVision.Services
{
    public interface IElspotService
    {
        Task<OperationResult> GetElspotDayAheadPricesAsync();
        Task<OperationResult> GetElspotPricesLast1500HourAsync();
        Task<OperationResult> GetElspotPricesLastYearHourAsync();
        Task<OperationResult> GetElspotPricesLast750HourAsync();
        Task<OperationResult> GetElspotLast60HPricesAsync();
    }
    public class ElspotService(HttpClient httpClient, ILogger<ElspotService> logger) : IElspotService
    {
        private readonly HttpClient httpClient = httpClient;
        private readonly ILogger<ElspotService> logger = logger;

        public async Task<OperationResult> GetElspotDayAheadPricesAsync()
        {
            try
            {
                var response = await httpClient.GetAsync("https://api.energidataservice.dk/dataset/Elspotprices?filter={%22pricearea%22:[%22DK1%22]}&sort=HourDK%20desc&limit=24");

                if (!response.IsSuccessStatusCode)
                {
                    return await ResponseHandlers.Handle_Unsuccessful_ResponseAsync(response);
                }

                return await ResponseHandlers.Handle_Successful_ResponseAsync<ElspotResponse>(response);
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

        public async Task<OperationResult> GetElspotLast60HPricesAsync()
        {
            try
            {
                var response = await httpClient.GetAsync("https://api.energidataservice.dk/dataset/Elspotprices?filter={%22pricearea%22:[%22DK1%22]}&sort=HourDK%20desc&limit=60");

                if (!response.IsSuccessStatusCode)
                {
                    return await ResponseHandlers.Handle_Unsuccessful_ResponseAsync(response);
                }

                return await ResponseHandlers.Handle_Successful_ResponseAsync<ElspotResponse>(response);
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

        public async Task<OperationResult> GetElspotPricesLast1500HourAsync()
        {
            try
            {
                var response = await httpClient.GetAsync("https://api.energidataservice.dk/dataset/Elspotprices?filter={%22pricearea%22:[%22DK1%22]}&sort=HourDK%20desc&limit=1500");

                if (!response.IsSuccessStatusCode)
                {
                    return await ResponseHandlers.Handle_Unsuccessful_ResponseAsync(response);
                }

                return await ResponseHandlers.Handle_Successful_ResponseAsync<ElspotResponse>(response);
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

        public async Task<OperationResult> GetElspotPricesLastYearHourAsync()
        {
            try
            {
                var response = await httpClient.GetAsync("https://api.energidataservice.dk/dataset/Elspotprices?filter={%22pricearea%22:[%22DK1%22]}&sort=HourDK%20desc&limit=8760");

                if (!response.IsSuccessStatusCode)
                {
                    return await ResponseHandlers.Handle_Unsuccessful_ResponseAsync(response);
                }

                return await ResponseHandlers.Handle_Successful_ResponseAsync<ElspotResponse>(response);
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

        public async Task<OperationResult> GetElspotPricesLast750HourAsync()
        {
            try
            {
                var response = await httpClient.GetAsync("https://api.energidataservice.dk/dataset/Elspotprices?filter={%22pricearea%22:[%22DK1%22]}&sort=HourDK%20desc&limit=750");

                if (!response.IsSuccessStatusCode)
                {
                    return await ResponseHandlers.Handle_Unsuccessful_ResponseAsync(response);
                }

                return await ResponseHandlers.Handle_Successful_ResponseAsync<ElspotResponse>(response);
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
    }
}
