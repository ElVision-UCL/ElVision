using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using ElVisionLibrary.Dictionaries;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Text;
using ApexCharts;
using Newtonsoft.Json;
using System.Reflection;
using ElVisionLibrary.Models.ElPris;
using ElVisionLibrary.Models.Utilities;
using ElVision.Utilities;
using ElVisionLibrary.Models.EnergiDataService;

namespace ElVision.Services
{
    public interface ITariffsService
    {
        Task<OperationResult> GetChargeOwnerTariffsAsync(string chargeOwner);
        Task<OperationResult> GetNationalTariffsAsync();
    }
    public class TariffsService(ILogger<TariffsService> logger) : ITariffsService
    {
        private static readonly HttpClient httpClient = new HttpClient();
        private readonly string baseUrl = "https://api.energidataservice.dk/dataset/DatahubPricelist";
        private readonly ILogger<TariffsService> logger = logger;

        public async Task<OperationResult> GetChargeOwnerTariffsAsync(string chargeOwner)
        {
            try
            {
                foreach (var entry in ChargeOwners.CHARGEOWNER)
                {
                    var test = entry.Value["company"].ToString();
                    if (chargeOwner.ToLower() == entry.Value["company"].ToString().ToLower())
                    {
                        chargeOwner = entry.Key.ToString();
                        break;
                    }
                }

                var chargeownerData = ChargeOwners.CHARGEOWNER[chargeOwner];
                string limit = "limit=500";
                var type = (List<string>)chargeownerData["type"];
                string combinedType = "";

                for (int i = 0; i < type.Count; i++)
                {
                    // Add double quotes around each type and a comma if it's not the last element.
                    combinedType += $"\"{type[i]}\"";
                    if (i < type.Count - 1)
                    {
                        combinedType += ",";
                    }
                }

                string result = combinedType;

                var chargedType = (List<string>)chargeownerData["chargetype"];
                string combinedChargeType = string.Join(",", chargedType);

                string objFilter = $"filter=%7B\"chargetypecode\":[{combinedType}],\"gln_number\":[\"{chargeownerData["gln"]}\"],\"chargetype\":[\"{combinedChargeType}\"]%7D";
                string sort = "sort=ValidFrom desc";
                string query = $"{objFilter}&{sort}&{limit}";

                var response = await httpClient.GetAsync($"{baseUrl}?{query}");

                if (!response.IsSuccessStatusCode)
                {
                    return await ResponseHandlers.Handle_Unsuccessful_ResponseAsync(response);
                }

                return await ResponseHandlers.Handle_Successful_ResponseAsync<TariffData>(response);
            }
            catch (HttpRequestException ex)
            {
                logger.LogError(ex, "Network error occurred while fetching the tariffs.");
                return ResponseHandlers.Handle_NetworkErrorResponse(ex);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An unexpected error occurred.");
                return ResponseHandlers.Handle_UnexpectedResponse(ex);
            }
        }

        public async Task<OperationResult> GetNationalTariffsAsync()
        {
            try
            {
                string searchFilter = "{\"Note\":[\"Elafgift\",\"Systemtarif\",\"Transmissions nettarif\"]}";
                string query = $"filter={searchFilter}&limit=500";

                var response = await httpClient.GetAsync($"{baseUrl}?{query}");

                if (!response.IsSuccessStatusCode)
                {
                    return await ResponseHandlers.Handle_Unsuccessful_ResponseAsync(response);
                }

                return await ResponseHandlers.Handle_Successful_ResponseAsync<TariffData>(response);
            }
            catch (HttpRequestException ex)
            {
                logger.LogError(ex, "Network error occurred while fetching the tariffs.");
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
