using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using WPF_MVVM_TEMPLATE.Entitys;
using WPF_MVVM_TEMPLATE.InterfaceAdapter;

namespace WPF_MVVM_TEMPLATE.Infrastructure
{
    // A service class that provides methods for interacting with web APIs.
    public class WebService : IWebService
    {
        private readonly HttpClient _httpClient;            
        private readonly string _baseUrl;                   
        private const int DefaultTimeoutInSeconds = 30; 
        private static WebService _instance;
        private string _authenticationHeader; 
        public string AuthenticationHeader {set { _authenticationHeader = value; } }

        public WebService(string baseUrl)
        {
            _baseUrl = baseUrl;
            _httpClient = GetHttpClient(_baseUrl); 
        }
        public static WebService GetInstance(string baseUrl)
        {
            if (_instance == null)
            {
                _instance = new WebService(baseUrl);
            }
            return _instance;
        }

        // Configures and returns a new HttpClient instance.
        private HttpClient GetHttpClient(string baseUrl)
        {
            HttpClient httpClient = new HttpClient();

            // Setting the base address for the HTTP client.
            httpClient.BaseAddress = new Uri(baseUrl);

            // Setting a default timeout for requests.
            httpClient.Timeout = TimeSpan.FromSeconds(DefaultTimeoutInSeconds);

            return httpClient;
        }

        /// <summary>
        /// Sends an HTTP GET request to the specified endpoint and retrieves the response.
        /// </summary>
        /// <param name="endpoint">API endpoint to send the GET request to.</param>
        /// <returns>A <see cref="ResponsPackage"/> containing the response details.</returns>
        public async Task<ResponsPackage> GetAsync(string endpoint)
        {
            AddCookie();
            
            try
            {
                using HttpResponseMessage response = await _httpClient.GetAsync(endpoint);
                return new ResponsPackage
                {
                    StatusCode = response.StatusCode,
                    ResponseBody = await response.Content.ReadAsStringAsync(),
                    Headers = response.Headers
                };
            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message);
                return new ResponsPackage
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    ResponseBody = $"Error: {err.Message}"
                };
            }
        }

        /// <summary>
        /// Sends an HTTP POST request to the specified endpoint with a payload.
        /// </summary>
        /// <param name="endpoint">The API endpoint to post data to.</param>
        /// <param name="payload">The data to send in the request body.</param>
        /// <returns>A <see cref="ResponsPackage"/> with the response details.</returns>
        public async Task<ResponsPackage> PostAsync(string endpoint, object payload)
        {
            AddCookie();
            try
            {
                var content = new StringContent(
                    JsonSerializer.Serialize(payload),
                    Encoding.UTF8,
                    "application/json"
                );

                using HttpResponseMessage response = await _httpClient.PostAsync(endpoint, content);

                return new ResponsPackage
                {
                    StatusCode = response.StatusCode,
                    ResponseBody = await response.Content.ReadAsStringAsync(),
                    Headers = response.Headers
                };
            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message);
                return new ResponsPackage
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    ResponseBody = $"Error: {err.Message}"
                };
            }
        }

        /// <summary>
        /// Sends an HTTP PUT request to the specified endpoint with a payload.
        /// </summary>
        /// <param name="endpoint">The API endpoint to send data to.</param>
        /// <param name="payload">The data to send in the request body.</param>
        /// <returns>A <see cref="ResponsPackage"/> with the response details.</returns>
        public async Task<ResponsPackage> PutAsync(string endpoint, object payload)
        {
            AddCookie();
            try
            {
                var content = new StringContent(
                    JsonSerializer.Serialize(payload),
                    Encoding.UTF8,
                    "application/json"
                );

                using HttpResponseMessage response = await _httpClient.PutAsync(endpoint, content);
                response.EnsureSuccessStatusCode();

                return new ResponsPackage
                {
                    StatusCode = response.StatusCode,
                    ResponseBody = await response.Content.ReadAsStringAsync(),
                    Headers = response.Headers
                };
            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message);
                return new ResponsPackage
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    ResponseBody = $"Error: {err.Message}"
                };
            }
        }

        /// <summary>
        /// Sends an HTTP DELETE request to the specified endpoint.
        /// </summary>
        /// <param name="endpoint">The API endpoint to send the DELETE request to.</param>
        /// <returns>A <see cref="ResponsPackage"/> with the response details.</returns>
        public async Task<ResponsPackage> DeleteAsync(string endpoint)
        {
            AddCookie();
            try
            {
                using HttpResponseMessage response = await _httpClient.DeleteAsync(endpoint);
                response.EnsureSuccessStatusCode();
                return new ResponsPackage
                {
                    StatusCode = response.StatusCode,
                    ResponseBody = await response.Content.ReadAsStringAsync(),
                    Headers = response.Headers
                };
            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message);
                return new ResponsPackage
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    ResponseBody = $"Error: {err.Message}"
                };
            }
        }

        public void AddCookie()
        {
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Add("Cookie", ".AspNetCore.Cookies="+_authenticationHeader);
        }

    }
}
