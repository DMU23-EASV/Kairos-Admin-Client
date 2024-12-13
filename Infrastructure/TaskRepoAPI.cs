using System.CodeDom.Compiler;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using WPF_MVVM_TEMPLATE.DTO;
using WPF_MVVM_TEMPLATE.Entitys;
using WPF_MVVM_TEMPLATE.InterfaceAdapter;

namespace WPF_MVVM_TEMPLATE.Infrastructure;

public class TaskRepoApi : ITaskRepo
{

    private readonly IWebService _webService;
    private const string ApiEndpoint = "api/Tasks"; 
    private const string ApiEndpointAwatingAwating = "api/admin/tasks/active"; 
    private const string ApiEndpointApproved = "api/admin/tasks/approved"; 

    public TaskRepoApi(IWebService webService)
    {
        _webService = webService;
    }
    
    /// <summary>
    /// Method for fetching all tasks from the api.
    /// </summary>
    /// <returns>collection of tasks or an empty collection if no tasks was found.</returns>
    public async Task<IEnumerable<TaskModel?>?> GetAllTasks()
    {
        // Fetcing all tasks from the api. 
        var response = await _webService.GetAsync(ApiEndpoint);
        
        // Error handeling 
        // if respoonse is null of contains no elements return an empty list. 
        if (response == null || response.ResponseBody == null || !response.ResponseBody.Any())
        {
            Console.WriteLine("API returned null, cannot get all tasks");
            return new List<TaskModel?>(); 
        }

        // if response is not OK. return an empty list. 
        if (response.StatusCode != HttpStatusCode.OK)
        {
            Console.WriteLine($"API returned status code {response.StatusCode}, cannot get all tasks");
            Console.WriteLine($"API returned body {response.ResponseBody}");
            return new List<TaskModel?>(); 
        }
        
        // if respons is OK, but no content. 
        if (response.StatusCode == HttpStatusCode.NoContent) return new List<TaskModel?>();

        // Deserializing response body. in case of content found.
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var taskList = JsonSerializer.Deserialize<List<TaskModel>>(response.ResponseBody, options);
        return taskList;
    }

    /// <summary>
    /// Method for updating a task
    /// </summary>
    /// <param name="task"></param>
    /// <param name="endpoint"></param>
    public async Task UpdateTask(TaskModel task, string endpoint)
    {
        
        var response = await _webService.PutAsync(endpoint, task);
        
        // evaluating response. 
        if (response == null || response.ResponseBody == null || !response.ResponseBody.Any())
        {
            Console.WriteLine("API returned null, cannot update task");
        }

        if (response?.StatusCode != HttpStatusCode.OK)
        {
            Console.WriteLine($"API returned status code {response?.StatusCode}, cannot update task");
        }
        
        Console.WriteLine($"API returned body {response?.ResponseBody}");
        
    }

    public async Task<int> GetAllTasksAwaitingApproval()
    {
        var response = await _webService.GetAsync(ApiEndpointAwatingAwating);
        
        // evaluating response. 
        if (response == null || response.ResponseBody == null || !response.ResponseBody.Any())
        {
            Console.WriteLine("API returned null");
            return -1;
        }

        if (response?.StatusCode != HttpStatusCode.OK)
        {
            Console.WriteLine($"API returned status code {response?.StatusCode}");
            return -1;
        }
        
        if (int.TryParse(response.ResponseBody, out int taskCount)) {
            return taskCount;
        }; 
        
        Console.WriteLine($"API returned status code {response.StatusCode}");
        return -1;
    }

    public async Task<int> GetAllTasksApproved()
    {
        var response = await _webService.GetAsync(ApiEndpointApproved);
        
        // evaluating response. 
        if (response == null || response.ResponseBody == null || !response.ResponseBody.Any())
        {
            Console.WriteLine("API returned null");
            return -1;
        }

        if (response?.StatusCode != HttpStatusCode.OK)
        {
            Console.WriteLine($"API returned status code {response?.StatusCode}");
            return -1;
        }
        
        if (int.TryParse(response.ResponseBody, out int taskCount)) {
            return taskCount;
        }; 
        
        Console.WriteLine($"API returned status code {response.StatusCode}");
        return -1;
    }

    public async Task<IEnumerable<TaskModel?>?> GetTasksByUsername(string username)
    {
        var response = await _webService.GetAsync($"/api/taskbyusername/{username}");
        
        if (response == null || response.ResponseBody == null || !response.ResponseBody.Any())
        {
            Console.WriteLine("API returned null");
            return new List<TaskModel?>();
        }

        if (response?.StatusCode != HttpStatusCode.OK)
        {
            Console.WriteLine($"API returned status code {response?.StatusCode}");
            return new List<TaskModel?>();
        }
        
        try
        {
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            JsonSerializer.Deserialize<List<TaskModel>>(response.ResponseBody, options);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return new List<TaskModel?>();
        }
        Console.WriteLine("API returned status code " + response.StatusCode);
        return new List<TaskModel?>();
    }
}