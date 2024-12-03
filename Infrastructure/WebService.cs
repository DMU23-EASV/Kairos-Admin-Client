using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using WPF_MVVM_TEMPLATE.Entitys;
using WPF_MVVM_TEMPLATE.InterfaceAdapter;

namespace WPF_MVVM_TEMPLATE.Infrastructure;

public class WebService : IWebService
{
    
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl;
    private const int DefaultTimeoutInSeconds = 30;
    
    public WebService(string baseUrl)
    {
        _baseUrl = baseUrl;
        _httpClient = GetHttpClient(_baseUrl);
    }

    private HttpClient GetHttpClient(string baseUrl)
    {
        HttpClient httpClient = new HttpClient();
        
        // Assigning base address.
        httpClient.BaseAddress = new Uri(baseUrl);
        
        // Configuring http client
        httpClient.Timeout = TimeSpan.FromSeconds(DefaultTimeoutInSeconds);
        
        return httpClient;
    }
    
    /// <summary>
    /// Method for calling an endpoint with get request. 
    /// </summary>
    /// <param name="endpoint"></param>
    /// <returns></returns>
    public async Task<ResponsPackage> GetAsync(string endpoint)
    {
        _httpClient.DefaultRequestHeaders.Accept.Clear();
        try
        {
            using HttpResponseMessage response = await _httpClient.GetAsync(endpoint);
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
    /// Method for calling an endpoint with POST request.
    /// </summary>
    /// <param name="endpoint">The API endpoint to post data to.</param>
    /// <param name="payload">The data to send in the request body.</param>
    /// <returns>A <see cref="ResponsPackage"/> with the response details.</returns>
    public async Task<ResponsPackage> PostAsync(string endpoint, object payload)
    {
        _httpClient.DefaultRequestHeaders.Accept.Clear();
        try
        {
            var content = new StringContent(
                JsonSerializer.Serialize(payload), 
                Encoding.UTF8, 
                "application/json"
            );

            using HttpResponseMessage response = await _httpClient.PostAsync(endpoint, content);
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


    public async Task<ResponsPackage> PutAsync(string endpoint, object payload)
    {
        _httpClient.DefaultRequestHeaders.Accept.Clear();
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

    public async Task<ResponsPackage> DeleteAsync(string endpoint)
    {
        _httpClient.DefaultRequestHeaders.Accept.Clear();
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
}