using ElVision.Data;
using ElVision.Utilities;
using ElVisionLibrary.Models.ElPris;
using ElVisionLibrary.Models.EnergiDataService;
using ElVisionLibrary.Models.Utilities;

namespace ElVision.Services
{
    public interface IBlobStorageService
    {
        Task<OperationResult> GetStandardConsumptionProfilesData();
        Task<OperationResult> GetNationalChargesData();
        Task<OperationResult> GetNordpoolData();
        Task<OperationResult> GetDistributionAreaData(string gridArea);
        Task<OperationResult> GetProductData(string gridArea);
        Task<OperationResult> GetZipCodeData();
        Task<OperationResult> GetClimateReportAsync(string priceArea);
    }

    public class BlobStorageService(HttpClient httpClient, IConfiguration configuration, ILogger<BlobStorageService> logger) : IBlobStorageService
    {
        private readonly HttpClient httpClient = httpClient;
        private readonly IConfiguration configuration = configuration;
        private readonly ILogger<BlobStorageService> logger = logger;

        public async Task<OperationResult> GetStandardConsumptionProfilesData()
        {
            try
            {
                var sasToken = configuration["SASToken"];
                var response = await httpClient.GetAsync($"https://elvisionsa.blob.core.windows.net/jsonfilesforelprices/configuration.json?{sasToken}");

                if (!response.IsSuccessStatusCode)
                {
                    return await ResponseHandlers.Handle_Unsuccessful_ResponseAsync(response);
                }

                return await ResponseHandlers.Handle_Successful_ResponseAsync<StandardConsumptionProfilesData>(response);
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

        public async Task<OperationResult> GetNationalChargesData()
        {
            try
            {
                var sasToken = configuration["SASToken"];
                var response = await httpClient.GetAsync($"https://elvisionsa.blob.core.windows.net/jsonfilesforelprices/nationalCharges.json?{sasToken}");

                if (!response.IsSuccessStatusCode)
                {
                    return await ResponseHandlers.Handle_Unsuccessful_ResponseAsync(response);
                }

                return await ResponseHandlers.Handle_Successful_ResponseAsync<NationalChargesData>(response);
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

        public async Task<OperationResult> GetClimateReportAsync(string priceArea)
        {
            try
            {
                var sasToken = configuration["SASToken"];
                var response = await httpClient.GetAsync($"https://elvisionsa.blob.core.windows.net/climatereport/ClimateReport.json?{sasToken}");

                if (!response.IsSuccessStatusCode)
                {
                    return await ResponseHandlers.Handle_Unsuccessful_ResponseAsync(response);
                }

                return await ResponseHandlers.Handle_Successful_ResponseAsync<List<ClimateRecord>>(response);
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

        public async Task<OperationResult> GetNordpoolData()
        {
            try
            {
                var sasToken = configuration["SASToken"];
                var response = await httpClient.GetAsync($"https://elvisionsa.blob.core.windows.net/jsonfilesforelprices/nordpools.json?{sasToken}");

                if (!response.IsSuccessStatusCode)
                {
                    return await ResponseHandlers.Handle_Unsuccessful_ResponseAsync(response);
                }

                return await ResponseHandlers.Handle_Successful_ResponseAsync<NordpoolData>(response);
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

        public async Task<OperationResult> GetProductData(string gridArea)
        {
            try
            {
                var sasToken = configuration["SASToken"];
                var response = await httpClient.GetAsync($"https://elvisionsa.blob.core.windows.net/jsonfilesforelprices/products_{gridArea}.json?{sasToken}");

                if (!response.IsSuccessStatusCode)
                {
                    return await ResponseHandlers.Handle_Unsuccessful_ResponseAsync(response);
                }

                return await ResponseHandlers.Handle_Successful_ResponseAsync<ProductData>(response);
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

        public async Task<OperationResult> GetDistributionAreaData(string gridArea)
        {
            try
            {
                var sasToken = configuration["SASToken"];
                var response = await httpClient.GetAsync($"https://elvisionsa.blob.core.windows.net/jsonfilesforelprices/distributionAreaCharge_{gridArea}.json?{sasToken}");

                if (!response.IsSuccessStatusCode)
                {
                    return await ResponseHandlers.Handle_Unsuccessful_ResponseAsync(response);
                }

                return await ResponseHandlers.Handle_Successful_ResponseAsync<DistributionAreaChargeData>(response);
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

        public async Task<OperationResult> GetZipCodeData()
        {
            try
            {
                var sasToken = configuration["SASToken"];
                var response = await httpClient.GetAsync($"https://elvisionsa.blob.core.windows.net/jsonfilesforelprices/static.json?{sasToken}");

                if (!response.IsSuccessStatusCode)
                {
                    return await ResponseHandlers.Handle_Unsuccessful_ResponseAsync(response);
                }

                return await ResponseHandlers.Handle_Successful_ResponseAsync<ZipCodeData>(response);
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
