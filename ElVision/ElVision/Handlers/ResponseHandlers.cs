using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Net;
using System.Text.RegularExpressions;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Reflection.Metadata;
using Newtonsoft.Json;
using ElVisionLibrary.Models.ElOverblik.MeteringPoint;
using ElVisionLibrary.Models.Utilities;

namespace ElVision.Utilities
{
    public static class ResponseHandlers
    {
        public static async Task<OperationResult> Handle_Unsuccessful_ResponseAsync(HttpResponseMessage response)
        {
            if (response.StatusCode == HttpStatusCode.InternalServerError)
            {
                return await Handle_InternalServerErrorResponseAsync(response);
            }

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return await Handle_NotFoundResponseAsync(response);
            }

            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                return await Handle_BadRequestResponseAsync(response);
            }

            return await Handle_UnexpectedResponseAsync(response);
        }

        public static async Task<OperationResult> Handle_Successful_ResponseAsync<T>(HttpResponseMessage response)
        {
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return await Handle_OKResponseAsync<T>(response);
            }

            if (response.StatusCode == HttpStatusCode.Created)
            {
                return await Handle_CreatedResponseAsync<T>(response);
            }

            if (response.StatusCode == HttpStatusCode.MultiStatus)
            {
                return await Handle_MultiStatusResponseAsync<T>(response);
            }

            return Handle_Successful_Response(response);
        }

        public static OperationResult Handle_Successful_Response(HttpResponseMessage response)
        {
            return new OperationResult
            {
                Success = true,
                StatusCode = response.StatusCode
            };
        }

        public static async Task<OperationResult> Handle_CreatedResponseAsync<T>(HttpResponseMessage response)
        {
            try
            {
                var data = await response.Content.ReadFromJsonAsync<T>();
                return new OperationResult<T>
                {
                    Success = true,
                    Data = data,
                    StatusCode = response.StatusCode
                };
            }
            catch (Exception e)
            {
                return new OperationResult
                {
                    Success = false,
                    ErrorMessage = $"There was a Created Response, but could not deserialize data as expected: {e.Message}",
                    StatusCode = response.StatusCode
                };
            }
        }

        public static async Task<OperationResult> Handle_OKResponseAsync<T>(HttpResponseMessage response)
        {
            try
            {
                var text = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<T>(text);
                return new OperationResult<T>
                {
                    Success = true,
                    Data = data,
                    StatusCode = response.StatusCode
                };
            }
            catch (Exception e)
            {
                return new OperationResult
                {
                    Success = false,
                    ErrorMessage = $"There was an OK Response, but could not deserialize data as expected: {e.Message}",
                    StatusCode = response.StatusCode
                };
            }
        }



        public static async Task<OperationResult> Handle_BadRequestResponseAsync(HttpResponseMessage response)
        {
            var content = await response.Content.ReadAsStringAsync();

            if (string.IsNullOrWhiteSpace(content))
            {
                return new OperationResult
                {
                    Success = false,
                    ErrorMessage = "Model state could not be validated: Response content is empty.",
                    StatusCode = HttpStatusCode.BadRequest
                };
            }

            if (content.ToLower().Contains("errors"))
            {
                try
                {
                    var modelStateErrors = await response.Content.ReadFromJsonAsync<ModelStateErrors>();

                    if (modelStateErrors?.Errors != null && modelStateErrors.Errors.Any())
                    {
                        return new OperationResult
                        {
                            Success = false,
                            ErrorMessage = "Model state could not be validated.",
                            ModelStateErrors = modelStateErrors.Errors,
                            StatusCode = HttpStatusCode.BadRequest
                        };
                    }

                    return new OperationResult
                    {
                        Success = false,
                        ErrorMessage = "Model state could not be validated: No specific errors found in the response.",
                        StatusCode = HttpStatusCode.BadRequest
                    };
                }
                catch (JsonException jsonEx)
                {
                    return new OperationResult
                    {
                        Success = false,
                        ErrorMessage = $"Error deserializing model state errors: {jsonEx.Message}",
                        StatusCode = HttpStatusCode.BadRequest
                    };
                }
            }

            return new OperationResult
            {
                Success = false,
                ErrorMessage = "Model state could not be validated: No 'errors' found in response content.",
                StatusCode = HttpStatusCode.BadRequest
            };
        }


        public static async Task<OperationResult> Handle_InternalServerErrorResponseAsync(HttpResponseMessage response)
        {

            var content = await response.Content.ReadAsStringAsync();
            if (content.Contains("JsonException"))
            {
                string pattern = @"System\.Text\.Json\.JsonException:\s([^\n]+)";

                var match = Regex.Match(content, pattern);

                var errorMessage = match.Groups[1].Value;
                return new OperationResult
                {
                    Success = false,
                    StatusCode = HttpStatusCode.InternalServerError,
                    ErrorMessage = errorMessage
                };
            }
            if (content.Contains("details"))
            {
                var problemDetails = await response.Content.ReadFromJsonAsync<ProblemDetails>();

                return new OperationResult
                {
                    Success = false,
                    ProblemDetails = problemDetails,
                    StatusCode = HttpStatusCode.InternalServerError,
                    ErrorMessage = "There was a problem on the server."
                };
            }

            return new OperationResult
            {
                Success = false,
                ErrorMessage = "There was an internal server error.",
                StatusCode = HttpStatusCode.InternalServerError
            };
        }

        public static async Task<OperationResult> Handle_MultiStatusResponseAsync<T>(HttpResponseMessage response)
        {
            try
            {
                var data = await response.Content.ReadFromJsonAsync<T>();

                return new OperationResult<T>
                {
                    Success = false,
                    Data = data,
                    StatusCode = HttpStatusCode.MultiStatus,
                    ErrorMessage = "There was a multi status response."
                };
            }
            catch (Exception e)
            {
                return new OperationResult
                {
                    Success = false,
                    ErrorMessage = $"There was a multi status response, but could not deserialize data as expected: {e.Message}",
                    StatusCode = HttpStatusCode.MultiStatus
                };
            }
        }

        public static OperationResult Handle_UnexpectedResponse(Exception ex)
        {
            return new OperationResult
            {
                Success = false,
                ErrorMessage = ex.Message,
                StatusCode = HttpStatusCode.InternalServerError
            };
        }

        public static OperationResult Handle_UnexpectedResponse()
        {
            return new OperationResult
            {
                Success = false,
                ErrorMessage = "Not sure what happened there my friend, my bad :( forgive me!",
                StatusCode = HttpStatusCode.InternalServerError
            };
        }

        public static async Task<OperationResult> Handle_UnexpectedResponseAsync(HttpResponseMessage response)
        {
            var content = await response.Content.ReadAsStringAsync();

            return new OperationResult
            {
                Success = false,
                ErrorMessage = $"An unexpected error has occured: {content}",
                StatusCode = HttpStatusCode.InternalServerError
            };
        }

        public static async Task<OperationResult> Handle_NotFoundResponseAsync(HttpResponseMessage response)
        {
            var content = await response.Content.ReadAsStringAsync();

            return new OperationResult
            {
                Success = false,
                ErrorMessage = $"No entity found in database, or no endpoint found: {content}",
                StatusCode = HttpStatusCode.NotFound
            };
        }

        public static OperationResult Handle_NetworkErrorResponse(HttpRequestException ex)
        {
            return new OperationResult
            {
                Success = false,
                ErrorMessage = ex.Message,
                StatusCode = HttpStatusCode.ServiceUnavailable
            };
        }
    }
}
