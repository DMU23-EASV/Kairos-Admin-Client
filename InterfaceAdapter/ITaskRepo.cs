using WPF_MVVM_TEMPLATE.Entitys;

namespace WPF_MVVM_TEMPLATE.InterfaceAdapter;

public interface ITaskRepo
{
    public Task<IEnumerable<TaskModel?>?> GetAllTasks();
    public Task UpdateTask(TaskModel task, string endpoint);
    public Task<int> GetAllTasksAwaitingApproval();
    public Task<int> GetAllTasksApproved();
}