using System.Collections.ObjectModel;
using System.IO;
using System.Net;
using System.Text;
using System.Windows;
using System.Windows.Input;
using Microsoft.Win32;
using WPF_MVVM_TEMPLATE.Entitys;
using WPF_MVVM_TEMPLATE.Entitys.DTOs;
using WPF_MVVM_TEMPLATE.Entitys.Enum;
using WPF_MVVM_TEMPLATE.Infrastructure;

namespace WPF_MVVM_TEMPLATE.Presentation.ViewModel;

public class ExportTaskViewModel : ViewModelBase
{
    public ObservableCollection<SimpleUserDTO> Users
    {
        get
        {
            return _users;
        }
        set
        {
            _users = value;
            OnPropertyChanged();
        }
    }
    private ObservableCollection<SimpleUserDTO> _users = new ObservableCollection<SimpleUserDTO>();

    public ObservableCollection<SimpleUserDTO> SelectedUsers
    {
        get
        {
            return _selectedUsers;
        }
        set
        {
            _selectedUsers = value;
            OnPropertyChanged();
        }
    }
    private ObservableCollection<SimpleUserDTO> _selectedUsers = new ObservableCollection<SimpleUserDTO>();
    
    private SimpleUserDTO _selectedUser;
    public SimpleUserDTO SelectedUser
    {
        get => _selectedUser;
        set
        {
            _selectedUser = value;
            OnPropertyChanged();
        }
    }
    
    private SimpleUserDTO _userSelected;
    public SimpleUserDTO UserSelected
    {
        get => _userSelected;
        set
        {
            _userSelected = value;
            OnPropertyChanged();
        }
    }
    public bool AnonymizeUsername { get; set; }
    public bool AnonymizeEmail { get; set; }
    public DateTime StartTime {
        get
        {
            return _startTime;
        }
        set
        {
            _startTime = value;
            OnPropertyChanged();
        }
    }
    private DateTime _startTime;
    
    public DateTime EndTime {
        get
        {
            return _endTime;
        }
        set
        {
            _endTime = value;
            OnPropertyChanged();
        }
    }
    private DateTime _endTime;
    
    public EFileTypes FileType { get; set; }
    
    public ObservableCollection<string> FileTypes {
        get
        {
            return _fileTypes;
        }
        set
        {
            _fileTypes = value;
            OnPropertyChanged();
        }
    }
    private ObservableCollection<string> _fileTypes;
    public string SelectedFileType { get; set; }
    
    private string FilePath
    {
        get
        {
            
            return FilePath;

        }
        set
        {
            
        }
    }

    public string SelectedStatus { get; set; }
    public ETaskModelStatus TaskStatus { get; set; }

    public ObservableCollection<string> TaskStatuses
    {
        get
        {
            return _taskStatuses;
        }
        set
        {
            _taskStatuses = value;
            OnPropertyChanged();
        }
    }
    private ObservableCollection<string> _taskStatuses = new ObservableCollection<string>();

    public ExportTaskViewModel()
    {
        PopulateUserList();
        PopulateFileTypes();
        PopulateStatusList();
        _startTime = DateTime.Now;
        _endTime = DateTime.Today.AddDays(1);
    }

    #region Populate on view load
    
    private void PopulateFileTypes()
    {
        List<EFileTypes> fileTypeList = Enum.GetValues(typeof(EFileTypes)).Cast<EFileTypes>().ToList();
        ObservableCollection<string> tempCollection = new ObservableCollection<string>();
        foreach (var enumFileType in fileTypeList)
        {
            tempCollection.Add(enumFileType.ToString());
        }
        _fileTypes = tempCollection;
        
    }

    private void PopulateStatusList()
    {
        List<ETaskModelStatus> statusList = Enum.GetValues(typeof(ETaskModelStatus)).Cast<ETaskModelStatus>().ToList();
        ObservableCollection<string> tempCollection = new ObservableCollection<string>();
        foreach (var status in statusList)
        {
            tempCollection.Add(status.ToString());
        }
        _taskStatuses = tempCollection;
    }

    private async void PopulateUserList()
    {
        try
        {
            var userRepo = new UserRepoApi(WebService.GetInstance("http://localhost:8080"));
            var userList = await userRepo.GetUsers();
            
            foreach (var user in userList)
            {
                _users.Add(new SimpleUserDTO {Username = user.Username, Email = user.Email});
                Console.WriteLine(user.Username);
            }
            
            
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error fetching users: {e.Message}");
        }
        
    }

    #endregion
    
    #region Move user from one list to the other

    public ICommand MoveAllUsersToSelectedCommand => new CommandBase(MoveAllUsersToSelected);

    private void MoveAllUsersToSelected(object obj)
    {
        foreach (var user in _users)
        {
            _selectedUsers.Add(user);
        }
        _users.Clear();
    }

    public ICommand MoveAllSelectedUsersCommand => new CommandBase(MoveAllSelectedUsers);

    private void MoveAllSelectedUsers(object obj)
    {
        foreach (var user in _selectedUsers)
        {
            _users.Add(user);
        }
        _selectedUsers.Clear();
    }

    public ICommand SelectUserCommand => new CommandBase(MoveUserToSelected);

    private void MoveUserToSelected(object obj)
    {
        if (SelectedUser != null)
        {
            _selectedUsers.Add(SelectedUser);
            _users.Remove(SelectedUser);
            SelectedUser = null;
        }
    }
    
    public ICommand FromSelectedToUserCommand => new CommandBase(MoveUserFromSelectedToUser);

    private void MoveUserFromSelectedToUser(object obj)
    {
        if (UserSelected != null)
        {
            _users.Add(UserSelected);
            _selectedUsers.Remove(UserSelected);
            UserSelected = null;
        }
    }
    
    #endregion

    public ICommand ExportTaskCommand => new CommandBase(ExportTask);

    private async void ExportTask(object obj)
    {
        // Needs to be moved, this is just for testing
        //await System.Windows.Application.Current.Dispatcher.InvokeAsync(() => ShowDialog());
        //Console.WriteLine(FilePath);
        
        if (string.IsNullOrEmpty(SelectedFileType)) return;

        switch (SelectedStatus)
        {
            case "Draft":
                TaskStatus = ETaskModelStatus.Draft;
                break;
            case "AwaitingApproval":
                TaskStatus = ETaskModelStatus.AwaitingApproval;
                break;
            case "Rejected":
                TaskStatus = ETaskModelStatus.Rejected;
                break;
            case "Approved":
                TaskStatus = ETaskModelStatus.Approved;
                break;
            default:
                Console.WriteLine("How did you get here?");
                break;
        }
        
        var taskRepo = new TaskRepoApi(WebService.GetInstance("http://localhost:8080"));

        List<ExportableTaskDTO> exportableTasks = new List<ExportableTaskDTO>();
        
        foreach (var user in _selectedUsers)
        {
            if (AnonymizeEmail)
            {
                user.Email = "--";
            }
            
            var tasks = await taskRepo.GetTasksForExport(user.Username, _startTime, _endTime, (int)TaskStatus);
            var taskList = tasks.ToList();
            Console.WriteLine(taskList.Count() + " Hello");

            if (taskList.Any())
            {
                foreach (var task in taskList)
                {
                    if (AnonymizeUsername)
                    {
                        if (task != null) task.Owner = "-";
                    }
                
                    exportableTasks.Add(new ExportableTaskDTO{Task = task, User = user});
                }
            }

        }
        
        switch (SelectedFileType)
        {
            case "JSON":
                FileType = EFileTypes.JSON;
                break;
            case "XML":
                FileType = EFileTypes.XML;
                break;
            case "CSV":
                FileType = EFileTypes.CSV;
                break;
            default:
                Console.WriteLine("How did you get here?");
                break;
        }
            
        switch (FileType)
        {
            case EFileTypes.JSON:
                ExportAsJson(exportableTasks);
                break;
            case EFileTypes.XML:
                ExportAsXml(exportableTasks);
                break;
            case EFileTypes.CSV:
                ExportAsCSV(exportableTasks);
                break;
            default:
                Console.WriteLine("How did you get here?");
                break;
        }
    }

    private void ExportAsCSV(List<ExportableTaskDTO> tasks)
    {

        if (tasks.Count == 0)
        {
            return;
        }
        
        var saveFileDialog = new SaveFileDialog
        {
            Filter = "CSV files (*.csv)|*.csv",
            FileName = "Tasks.csv",
            Title = "Save Tasks as CSV"
        };

        if (saveFileDialog.ShowDialog() == true)
        {
            string filePath = saveFileDialog.FileName;

            try
            {
                var csvContent = new StringBuilder();
                
                // Adding headers
                csvContent.AppendLine("name,owner,email,start_time,end_time,start_kilometer,end_kilometer");

                foreach (var task in tasks)
                {
                    csvContent.AppendLine($"\"{task.Task.Name}\"," +
                                          $"\"{task.User.Username}\"," +
                                          $"\"{task.User.Email}\"," +
                                          $"\"{task.Task.StartTime:yyyy-MM-dd HH:mm:ss}\"," + 
                                          $"\"{task.Task.EndTime:yyyy-MM-dd HH:mm:ss}\"," +
                                          $"\"{task.Task.StartKilometers}\"," +
                                          $"\"{task.Task.EndKilometers}\"");
                }

                Console.WriteLine(csvContent.ToString());
                File.WriteAllText(filePath, csvContent.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        
    }
    
    private void ExportAsXml(List<ExportableTaskDTO> tasks)
    {
        throw new NotImplementedException();
    }

    private void ExportAsJson(List<ExportableTaskDTO> tasks)
    {
        throw new NotImplementedException();
    }
}