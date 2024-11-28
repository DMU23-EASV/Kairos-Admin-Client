using System.Net.Http;
using System.Runtime.InteropServices.JavaScript;
using System.Text.Json;
using System.Windows.Documents;
using WPF_MVVM_TEMPLATE.DTO;
using WPF_MVVM_TEMPLATE.InterfaceAdapter;

namespace WPF_MVVM_TEMPLATE.Application;

public class LoadUsers
{
    
    private IWebService _webService;
    public LoadUsers(IWebService webService)
    {
        _webService = webService;
    }

    /// <summary>
    /// Method for loading users from a database. 
    /// </summary>
    /// <returns>Returns users or if no useres returns null</returns>
    public async Task<List<ManageUserDTO?>> FetchUsers()
    {
        try
        {
            using HttpResponseMessage response = await _webService.GetAsync("/api/users");
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            // Above three lines can be replaced with new helper method below
            // string responseBody = await client.GetStringAsync(uri);
            
            Console.WriteLine($"Response: {response.StatusCode}");
            
            // in case of sucessfull requst, but no contet found. 
            if (response.StatusCode == System.Net.HttpStatusCode.NoContent) return new List<ManageUserDTO>();
            
            Console.WriteLine($"Response: Passed");
            
            // in case of content found.
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var userList = JsonSerializer.Deserialize<List<ManageUserDTO>>(responseBody, options);
            Console.WriteLine($"UserList: {JsonSerializer.Serialize(userList, options)}");
            return userList;
            
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine("\nException Caught!");
            Console.WriteLine("Message :{0} ", e.Message);
        }
        return null;
    }
    
    
}