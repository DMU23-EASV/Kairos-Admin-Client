using System.Net;
using System.Text.Json;
using WPF_MVVM_TEMPLATE.DTO;
using WPF_MVVM_TEMPLATE.Entitys;
using WPF_MVVM_TEMPLATE.InterfaceAdapter;

namespace WPF_MVVM_TEMPLATE.Infrastructure;

public class TaskRepoApi : ITaskRepo
{

    private readonly IWebService _webService;
    private const string ApiEndpoint = "api/Tasks"; 

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
    public async Task UpdateTask(TaskModel task)
    {
        
        
        
    }
    
}