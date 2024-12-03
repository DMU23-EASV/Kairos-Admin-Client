using WPF_MVVM_TEMPLATE.Entitys;

namespace WPF_MVVM_TEMPLATE.InterfaceAdapter;

public interface ITaskRepo
{
    public Task<IEnumerable<TaskModel?>?> GetAllTasks();
}