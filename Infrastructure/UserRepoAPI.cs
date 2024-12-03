using System.Text.Json;
using WPF_MVVM_TEMPLATE.DTO;
using WPF_MVVM_TEMPLATE.InterfaceAdapter;

namespace WPF_MVVM_TEMPLATE.Infrastructure;

public class UserRepoApi : IUserRepo
{
    private IWebService _webService;
 
    public UserRepoApi(IWebService webService)
    {
        _webService = webService;
    }
    
    /// <summary>
    /// Method for loading users from a database. 
    /// </summary>
    /// <returns>Returns List<ManageUserDTO> or if no useres returns null</returns>
    public async Task<List<ManageUserDTO?>> GetUsers()
    {
        // Fetching all useres from api endpoint.
        var response = await _webService.GetAsync("/api/users");

        // In case of server error, return empty string. 
        if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
        {
            Console.WriteLine($"Internal Server Error {response.StatusCode}, {response.ResponseBody}" );
            return new List<ManageUserDTO?>();
        };
        
        // in case of sucessfull requst, but no contet found. 
        if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
        {
            Console.WriteLine($"No Content {response.StatusCode}, {response.ResponseBody}" );
            return new List<ManageUserDTO?>();
        };
        
        // checking if responsbody has data. 
        if (response.ResponseBody == null || response.ResponseBody.Length <= 0 || string.IsNullOrWhiteSpace(response.ResponseBody)) return new List<ManageUserDTO?>();
        
        // in case of content found.
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var userList = JsonSerializer.Deserialize<List<ManageUserDTO>>(response.ResponseBody, options);
        return userList;
    }
    
    
}