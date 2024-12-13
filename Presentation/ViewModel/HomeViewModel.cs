using System.Collections.ObjectModel;
using System.Windows.Input;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.Themes;
using SkiaSharp;
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
        GetUsersForChartAsync();
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


    #region Chart Section
    
    
    
     private readonly UserRepoApi _userRepoApi;
    private readonly TaskRepoApi _taskRepoApi;

    public ObservableCollection<ISeries> Series { get; set; } = new ObservableCollection<ISeries>();
    public ObservableCollection<Axis> XAxes { get; set; } = new ObservableCollection<Axis>
    {
        new Axis
        {
            Labels = new List<string>(),
            SeparatorsPaint = new SolidColorPaint(SKColors.White)
        }
    };
    public ObservableCollection<Axis> YAxes { get; set; } = new ObservableCollection<Axis>
    {
        new Axis { IsVisible = false }
    };

    public bool IsReading { get; set; } = true;

    public async Task GetUsersForChartAsync()
    {
        var loadUsers = new LoadUsers(_userRepoApi);
        var users = await loadUsers.GetUsers();

        foreach (var user in users)
        {
            var loadTasks = new LoadTasks(_taskRepoApi);
            var tasks = await loadTasks.GetTaskByUsername(user.Username);

            // Add user data to the chart
            var taskCount = tasks.Count();
            AddOrUpdateUser(user.Username, taskCount);
        }
    }

    private void AddOrUpdateUser(string username, int value)
    {
        var barSeries = Series.OfType<ColumnSeries<int>>().FirstOrDefault(s => s.Name == username);

        if (barSeries != null)
        {
            // Update the existing bar's value
            if (barSeries.Values is ObservableCollection<int> values)
            {
                values[0] = value;
            }
        }
        else
        {
            // Add a new bar for the user
            Series.Add(new ColumnSeries<int>
            {
                Name = username,
                Values = new ObservableCollection<int> { value },
                Fill = new SolidColorPaint(SKColors.Blue)
            });

            // Add username to XAxis labels
            XAxes[0].Labels.Add(username);
        }
    }

    public async Task StartRaceAsync()
    {
        await Task.Delay(1000);

        while (IsReading)
        {
            await GetUsersForChartAsync();

            // Simulate delay for race chart update
            await Task.Delay(2000);
        }
    }

    #endregion


}