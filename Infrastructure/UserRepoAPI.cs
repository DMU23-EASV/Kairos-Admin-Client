using System.Net.Http;
using System.Text;
using System.Text.Json;
using WPF_MVVM_TEMPLATE.DTO;
using WPF_MVVM_TEMPLATE.Entitys.DTOs;
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
        if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError) return new List<ManageUserDTO?>();
        
        // in case of sucessfull requst, but no contet found. 
        if (response.StatusCode == System.Net.HttpStatusCode.NoContent) return new List<ManageUserDTO?>();
        
        // checking if responsbody has data. 
        if (response.ResponseBody == null || response.ResponseBody.Length <= 0) return new List<ManageUserDTO?>();
        
        // in case of content found.
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var userList = JsonSerializer.Deserialize<List<ManageUserDTO>>(response.ResponseBody, options);
        return userList;
    }
    
    public async Task<CreateUserDTO?> CreateUser(CreateUserDTO user)
    {
        // Sending the user data as payload via the POST request.
        var response = await _webService.PostAsync("/api/create", user);

        // Handling different response scenarios.

        // If the server responds with an internal server error, throw an exception.
        if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
        {
            throw new Exception($"Server error: {response.ResponseBody}");
        }

        // If the request is successful and returns content, deserialize the response body.
        if (response.StatusCode == System.Net.HttpStatusCode.Created || response.StatusCode == System.Net.HttpStatusCode.OK)
        {
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            return JsonSerializer.Deserialize<CreateUserDTO>(response.ResponseBody, options);
        }

        // Handle unexpected response status codes by throwing an exception.
        throw new Exception($"Unexpected response: {response.StatusCode}, {response.ResponseBody}");
    }

    public async Task<FullUserDTO?> GetUserByEmail(string email)
    {
        var response = await _webService.GetAsync("/api/userUsername/" + email);

        if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
        {
            throw new Exception($"Server error: {response.ResponseBody}");
        }

        if (response.StatusCode == System.Net.HttpStatusCode.NoContent || response.ResponseBody == null || response.ResponseBody.Length <= 0)
        {
            throw new Exception($"No data or data empty");
        }

        if (response.StatusCode == System.Net.HttpStatusCode.Created ||
            response.StatusCode == System.Net.HttpStatusCode.OK)
        {
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            return JsonSerializer.Deserialize<FullUserDTO>(response.ResponseBody, options);
        }
        // Handle unexpected response status codes by throwing an exception.
        throw new Exception($"Unexpected response: {response.StatusCode}, {response.ResponseBody}");
    }

    public async Task<FullUserDTO?> GetUserByUsername(string username)
    {
        var response = await _webService.GetAsync("/api/userUsername/" + username);

        if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
        {
            throw new Exception($"Server error: {response.ResponseBody}");
        }

        if (response.StatusCode == System.Net.HttpStatusCode.NoContent || response.ResponseBody == null || response.ResponseBody.Length <= 0)
        {
            throw new Exception($"No data or data empty");
        }

        if (response.StatusCode == System.Net.HttpStatusCode.Created ||
            response.StatusCode == System.Net.HttpStatusCode.OK)
        {
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            return JsonSerializer.Deserialize<FullUserDTO>(response.ResponseBody, options);
        }
        // Handle unexpected response status codes by throwing an exception.
        throw new Exception($"Unexpected response: {response.StatusCode}, {response.ResponseBody}");
    }
}