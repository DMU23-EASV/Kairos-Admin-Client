using System.Net.Http;
using WPF_MVVM_TEMPLATE.InterfaceAdapter;

namespace WPF_MVVM_TEMPLATE.Infrastructure;

public class WebService : IWebService
{
    
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl;
    
    
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
        return httpClient;
    }
    
    public async Task<HttpResponseMessage> GetAsync(string endpoint)
    {
        _httpClient.DefaultRequestHeaders.Accept.Clear();
        return await _httpClient.GetAsync(endpoint);
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