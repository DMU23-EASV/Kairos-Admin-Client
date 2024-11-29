using System.Net;
using System.Net.Http;
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

    public Task PostAsync(string url)
    {
        throw new NotImplementedException();
    }

    public Task PutAsync(string url)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(string url)
    {
        throw new NotImplementedException();
    }
}