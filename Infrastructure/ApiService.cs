using System.Net.Http;
using System.Text;
using System.Text.Json;
using WPF_MVVM_TEMPLATE.Entitys.DTOs;
using WPF_MVVM_TEMPLATE.InterfaceAdapter;

namespace WPF_MVVM_TEMPLATE.Infrastructure;

public class ApiService : IApiService
{
    private readonly HttpClient _httpClient;

    public ApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    
    //TODO: Find en bedre måde at holde URL til vores end Points.
    public async Task<CreateUserDTO?> CreateUserAsync(CreateUserDTO user)
    {
        var content = new StringContent(JsonSerializer.Serialize(user), Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync("http://localhost:8080/api/create", content);
        Console.WriteLine(content.ReadAsStringAsync().Result);
        Console.WriteLine("!!!SERVER API!!! : " + response);
        if (response.IsSuccessStatusCode)
        {
            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<CreateUserDTO>(responseContent);
        }

        throw new Exception(await response.Content.ReadAsStringAsync());
    }
}