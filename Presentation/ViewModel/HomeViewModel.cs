using System.Windows.Input;
using WPF_MVVM_TEMPLATE.Application;
using WPF_MVVM_TEMPLATE.Infrastructure;
using WPF_MVVM_TEMPLATE.InterfaceAdapter;

namespace WPF_MVVM_TEMPLATE.Presentation.ViewModel;

public class HomeViewModel : ViewModelBase
{
    public int TaskAwaitingApproval
    {
        get => _taskAwaitingApproval;
        set
        {
            _taskAwaitingApproval = value;
            OnPropertyChanged();
        }
    }
    private int _taskAwaitingApproval { get; set; } = 0; 
    public int TaskApproved
    {
        get => _taskApproved;
        set
        {
            _taskApproved = value;
            OnPropertyChanged();
        }
    }
    private int _taskApproved { get; set; } = 0;
    private readonly LoadTasks _loadTasks;
    private ITaskRepo _taskRepo;
    private IWebService _webService;
    public ICommand NavigateToTaskCommand = new CommandBase(LoadTaskView);

    
    public HomeViewModel()
    {
        // initializing application layer.
        _webService = new WebService("http://localhost:8080");
        _taskRepo = new TaskRepoApi(_webService);
        _loadTasks = new LoadTasks(_taskRepo);
        LoadTasks();
    }

    private async void LoadTasks()
    {
        TaskAwaitingApproval = await _loadTasks.GetNumberTasksAwaitingApproval();
        TaskApproved = await _loadTasks.GetNumberTasksApproved();
    }
    
    private static void LoadTaskView(object obj)
    {
        ViewModelController.Instance.SetCurrentViewModel<EditTaskViewModel>();
    }


}