using Microsoft.Identity.Web;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using BusinessAssistAPIClient.Models;
using BusinessAssistAPIClient.Utils;
using JsonSerializer = System.Text.Json.JsonSerializer;
using Microsoft.Identity.Client;
using static System.Formats.Asn1.AsnWriter;

namespace BusinessAssistAPIClient.Services
{
    public static class BAServiceExtensions
    {
        public static void AddBAService(this IServiceCollection services)
        {
            // https://docs.microsoft.com/en-us/dotnet/standard/microservices-architecture/implement-resilient-applications/use-httpclientfactory-to-implement-resilient-http-requests
            services.AddHttpClient<IBAService, BAService>();
        }
    }

    public class BAService : IBAService
    {
        private readonly HttpClient _httpClient;
        private readonly string _BAServiceScope = string.Empty;
        private readonly string _BABaseAddress = string.Empty;
        private readonly string _ApiVersion = string.Empty;
        private readonly ITokenAcquisition _tokenAcquisition;

        public BAService(ITokenAcquisition tokenAcquisition, HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _tokenAcquisition = tokenAcquisition;
            _BAServiceScope = configuration["BusinessAssist:BAServiceScope"];
            _BABaseAddress = configuration["BusinessAssist:BABaseAddress"];
            _ApiVersion = configuration["BusinessAssist:ApiVersion"];

            if (!string.IsNullOrEmpty(_BABaseAddress))
            {
                if (!_BABaseAddress.EndsWith("/"))
                {
                    _BABaseAddress = _BABaseAddress + "/";
                }
            }
        }

        /// <summary>
        /// Generates the App User Token using the Business Assist Scope
        /// </summary>
        /// <returns></returns>
        private async Task PrepareAuthenticatedClient()
        {
            var accessToken = await _tokenAcquisition.GetAccessTokenForUserAsync(new[] { _BAServiceScope });
            Debug.WriteLine($"Access Token - {accessToken}");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="parameterName"></param>
        /// <returns></returns>
        private static string GetParameter(IEnumerable<string> parameters, string parameterName)
        {
            int offset = parameterName.Length + 1;
            return parameters.FirstOrDefault(p => p.StartsWith($"{parameterName}="))?.Substring(offset)?.Trim('"');
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="HttpRequestException"></exception>
        public async Task<BAValidateTenantModel> GetValidateTenantAsync()
        {
            await PrepareAuthenticatedClient();

            string version = _ApiVersion;

            var response = await _httpClient.GetAsync($"{_BABaseAddress}/api/{version}/Validate/Tenant");
            if (response.StatusCode == HttpStatusCode.OK)
            {
                BAValidateTenantModel baResponse = new BAValidateTenantModel();
                baResponse.Status = "HttpStatusCode.OK";
                string content = await response.Content.ReadAsStringAsync();
                var jsonElement = JsonSerializer.Deserialize<JsonElement>(content);

                // The User AuthN token that was used for the authentication
                baResponse.Token = response.RequestMessage.Headers.Authorization.Parameter;

                // Dynamic object containing the details of the user pulled from the MS Graph
                baResponse.JsonResponse = JsonConvert.DeserializeObject<dynamic>(content);

                // The raw Response output of the request
                baResponse.Response = JsonSerializer.Serialize(jsonElement, new JsonSerializerOptions { WriteIndented = true });

                return baResponse;
            }
            else
            {
                throw new HttpRequestException($"Invalid status code in the HttpResponseMessage: {response.StatusCode}.");
            }

            throw new HttpRequestException($"Invalid status code in the HttpResponseMessage: {response.StatusCode}.");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="selfHelp"></param>
        /// <returns></returns>
        public async Task<BASelfHelpModel> GetSelfHelpAsync(BASelfHelpModel selfHelp)
        {
            if (!selfHelp.QueryText.IsNullOrEmpty())
            {
                string version = _ApiVersion;

                await PrepareAuthenticatedClient();

                var jsonRequest = JsonConvert.SerializeObject(selfHelp);
                var jsoncontent = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
                var response = await this._httpClient.PostAsync($"{_BABaseAddress}api/{version}/SelfHelp/insights", jsoncontent);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var jsonElement = JsonSerializer.Deserialize<JsonElement>(content);

                    selfHelp.Results = JsonSerializer.Deserialize<SelfHelpResults>(content, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });


                    selfHelp.Response = JsonSerializer.Serialize(jsonElement, new JsonSerializerOptions { WriteIndented = true });
                }
                else
                {
                    throw new HttpRequestException($"Invalid status code in the HttpResponseMessage: {response.StatusCode}.");
                }

                selfHelp.Status = response.StatusCode.ToString();
            }
            return selfHelp;
            //throw new HttpRequestException($"Invalid status code in the HttpResponseMessage: {response.StatusCode}.");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="forecastData"></param>
        /// <returns></returns>
        public async Task<BAForecastDataModel> GetForecastDataAsync(BAForecastDataModel forecastData)
        {
            if (!forecastData.QueryText.IsNullOrEmpty())
            {
                string version = _ApiVersion;
                string forecastOpId = forecastData.QueryText;

                await PrepareAuthenticatedClient();

                var response = await this._httpClient.GetAsync($"{_BABaseAddress}api/{version}/forecast/{forecastOpId}/download");

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var jsonElement = JsonSerializer.Deserialize<JsonElement>(content);
                    forecastData.ForecastData = JsonSerializer.Deserialize<ForecastDataResults>(content, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                    forecastData.Response = JsonSerializer.Serialize(jsonElement, new JsonSerializerOptions { WriteIndented = true });
                }
                else if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var jsonElement = JsonSerializer.Deserialize<JsonElement>(content);
                    //forecastData.ForecastData = JsonSerializer.Deserialize<ForecastDataResults>(content, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                    forecastData.Response = JsonSerializer.Serialize(jsonElement, new JsonSerializerOptions { WriteIndented = true });
                }
                else
                {
                    throw new HttpRequestException($"Invalid status code in the HttpResponseMessage: {response.StatusCode}.");
                }

                forecastData.Status = response.StatusCode.ToString();
            }

            return forecastData;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="forecastData"></param>
        /// <returns></returns>
        public async Task<BAForecastDataModel> GetForecastOpStatusAsync(BAForecastDataModel forecastData)
        {
            if (!forecastData.QueryText.IsNullOrEmpty())
            {
                string version = _ApiVersion;
                string forecastOpId = forecastData.QueryText;

                await PrepareAuthenticatedClient();

                var response = await this._httpClient.GetAsync($"{_BABaseAddress}api/{version}/forecast/{forecastOpId}");

                if (response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.Accepted)
                {

                    var content = await response.Content.ReadAsStringAsync();
                    var jsonElement = JsonSerializer.Deserialize<JsonElement>(content);

                    forecastData.OperationStatus = JsonSerializer.Deserialize<ForecastOpStatus>(content, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                    forecastData.Response = JsonSerializer.Serialize(jsonElement, new JsonSerializerOptions { WriteIndented = true });
                }
                else
                {
                    throw new HttpRequestException($"Invalid status code in the HttpResponseMessage: {response.StatusCode}.");
                }

                forecastData.Status = response.StatusCode.ToString();
            }

            return forecastData;
            //throw new HttpRequestException($"Invalid status code in the HttpResponseMessage: {response.StatusCode}.");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="forecastData"></param>
        /// <returns></returns>
        public async Task<BAForecastDataModel> CreateForecastAsync(BAForecastDataModel forecastData)
        {
            if (!forecastData.Name.IsNullOrEmpty())
            {
                string version = _ApiVersion;
                string forecastOpId = forecastData.QueryText;

                await PrepareAuthenticatedClient();

                BACreateForecastDataModel inputData = forecastData.GetForecastInputData();

                var jsonRequest = JsonConvert.SerializeObject(inputData);
                var jsoncontent = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
                var response = await this._httpClient.PostAsync($"{_BABaseAddress}api/{version}/forecast?inputDataMethod=0", jsoncontent);

                if (response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.Accepted)
                {

                    var content = await response.Content.ReadAsStringAsync();
                    var jsonElement = JsonSerializer.Deserialize<JsonElement>(content);
                    forecastData.OperationStatus = JsonSerializer.Deserialize<ForecastOpStatus>(content, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                    forecastData.Response = JsonSerializer.Serialize(jsonElement, new JsonSerializerOptions { WriteIndented = true });
                }
                else
                {
                    throw new HttpRequestException($"Invalid status code in the HttpResponseMessage: {response.StatusCode}.");
                }


                forecastData.Status = response.StatusCode.ToString();
            }

            return forecastData;

        }
    }
}