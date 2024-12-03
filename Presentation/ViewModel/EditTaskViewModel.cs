using System.Collections.ObjectModel;
using System.Windows.Input;
using WPF_MVVM_TEMPLATE.Entitys;

namespace WPF_MVVM_TEMPLATE.Presentation.ViewModel;

public class EditTaskViewModel : ViewModelBase
{
    
    public ICommand SortByArchived = new CommandBase(c);
    public ICommand SortByAwaitingApproval = new CommandBase(c);
    public ICommand SortByApproved = new CommandBase(c);
    public ICommand SortByRejected = new CommandBase(c);
    private TaskModel? _selectedTask;
    public TaskModel? SelectedTask  { get => _selectedTask; set { _selectedTask = value; OnPropertyChanged(); } }
    public ObservableCollection<TaskModel>? TaskCollection { get; set; }

    public EditTaskViewModel()
    {
        TaskCollection = new ObservableCollection<TaskModel>();
    }
    
    
    private static void c(object obj)
    {
        Console.WriteLine(obj.ToString());
    }
}