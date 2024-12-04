using WPF_MVVM_TEMPLATE.Entitys;
using WPF_MVVM_TEMPLATE.InterfaceAdapter;

namespace WPF_MVVM_TEMPLATE.Application;

public class UpdateTask
{
    
    private ITaskRepo _taskRepo;
        
    public UpdateTask(ITaskRepo taskRepo)
    {
        _taskRepo = taskRepo;
    }
    
    public async Task UpdateTaskAsync(TaskModel task, string endpoint) => await _taskRepo.UpdateTask(task, endpoint);
}