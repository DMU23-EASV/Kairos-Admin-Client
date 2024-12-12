using WPF_MVVM_TEMPLATE.Entitys;
using WPF_MVVM_TEMPLATE.InterfaceAdapter;

namespace WPF_MVVM_TEMPLATE.Application.Utility;

public class ExportTasks
{
    private ITaskRepo _taskRepo;

    public ExportTasks(ITaskRepo taskRepo)
    {
        _taskRepo = taskRepo;
    }

    public async Task<IEnumerable<TaskModel?>?> ExportAsync(string username, DateTime startDate, DateTime endDate, int statusInt, string endpoint) => 
        await _taskRepo.GetTasksForExport( username, startDate, endDate, statusInt);
}