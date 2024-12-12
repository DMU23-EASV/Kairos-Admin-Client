using WPF_MVVM_TEMPLATE.Entitys;
using WPF_MVVM_TEMPLATE.InterfaceAdapter;

namespace WPF_MVVM_TEMPLATE.Application;

public class LoadTasks 
{

    private ITaskRepo _repo;
    public LoadTasks(ITaskRepo taskRepo)
    {
        _repo = taskRepo;
    }
    public async Task<IEnumerable<TaskModel?>?> GetAllTasks() => await _repo.GetAllTasks();
    public async Task<int> GetNumberTasksAwaitingApproval() => await _repo.GetAllTasksAwaitingApproval();
    public async Task<int> GetNumberTasksApproved() => await _repo.GetAllTasksApproved();
}