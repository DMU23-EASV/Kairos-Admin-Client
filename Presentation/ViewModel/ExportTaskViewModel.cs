using System.Collections.ObjectModel;
using System.IO;
using System.Linq.Expressions;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Windows;
using System.Windows.Input;
using Microsoft.Win32;
using MongoDB.Bson.IO;
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
    public bool AnonymizeUsername
    {
        get
        {
            return _anonymizeUsername;
        }
        set
        {
            _anonymizeUsername = value;
            OnPropertyChanged();
        }
    }
    private bool _anonymizeUsername;
    
    public bool AnonymizeEmail {
        get
        {
            return _anonymizeEmail;
        }
        set
        {
            _anonymizeEmail = value;
            OnPropertyChanged();
        }
    }
    private bool _anonymizeEmail;
    
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
    
    /// <summary>
    /// Method for populating the filetype combobox
    /// </summary>
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

    /// <summary>
    /// Method for populating the task status combobox
    /// </summary>
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

    /// <summary>
    /// Method for populating the view list with selectable users.
    /// </summary>
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

    /// <summary>
    /// Method for moving all selectable users to the selected list view.
    /// </summary>
    /// <param name="obj"></param>
    private void MoveAllUsersToSelected(object obj)
    {
        foreach (var user in _users)
        {
            _selectedUsers.Add(user);
        }
        _users.Clear();
    }

    public ICommand MoveAllSelectedUsersCommand => new CommandBase(MoveAllSelectedUsers);

    /// <summary>
    /// Method for moving all users in the selected view list to the view list containing selectable users.
    /// </summary>
    /// <param name="obj"></param>
    private void MoveAllSelectedUsers(object obj)
    {
        foreach (var user in _selectedUsers)
        {
            _users.Add(user);
        }
        _selectedUsers.Clear();
    }

    public ICommand SelectUserCommand => new CommandBase(MoveUserToSelected);

    /// <summary>
    /// Method for moving all selected user to the selected list view.
    /// </summary>
    /// <param name="obj"></param>
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

    
    /// <summary>
    /// Method for moving selected users from the selected list view to the selectable list view.
    /// </summary>
    /// <param name="obj"></param>
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

    
    /// <summary>
    /// Method for Exporting tasks from the selected users.
    /// </summary>
    /// <param name="obj"></param>
    private async void ExportTask(object obj)
    {
        
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
            string tempEmail = user.Email;
            if (_anonymizeEmail)
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
                    if (_anonymizeUsername)
                    {
                        if (task != null) task.Owner = "-";
                    }
                
                    exportableTasks.Add(new ExportableTaskDTO
                    {
                        Name = task.Name,
                        Owner = task.Owner,
                        Email = user.Email,
                        StartTime = task.StartTime,
                        EndTime = task.EndTime,
                        StartKilometers = task.StartKilometers,
                        EndKilometers = task.EndKilometers
                    });
                }
            }
            user.Email = tempEmail;
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

    #region Export as CSV
    
    /// <summary>
    /// Method for handling the export for when CSV filetype is selected.
    /// </summary>
    /// <param name="tasks"></param>
    private void ExportAsCSV(List<ExportableTaskDTO> tasks)
    {

        if (tasks.Count == 0)
        {
            // TODO: Replace with notification system
            MessageBox.Show("There are no tasks to export.");
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
                    csvContent.AppendLine($"\"{task.Name}\"," +
                                          $"\"{task.Owner}\"," +
                                          $"\"{task.Email}\"," +
                                          $"\"{task.StartTime:yyyy-MM-dd HH:mm:ss}\"," + 
                                          $"\"{task.EndTime:yyyy-MM-dd HH:mm:ss}\"," +
                                          $"\"{task.StartKilometers}\"," +
                                          $"\"{task.EndKilometers}\"");
                }
                
                File.WriteAllText(filePath, csvContent.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        
    }
    
    #endregion

    #region Export as XML
    
    /// <summary>
    /// Method for handling the export when XML filetype is selected.
    /// </summary>
    /// <param name="tasks"></param>
    /// <exception cref="NotImplementedException"></exception>
    private void ExportAsXml(List<ExportableTaskDTO> tasks)
    {
        MessageBox.Show("This is WIP");
    }

    #endregion

    #region Export as JSON
    
    /// <summary>
    /// Methof for handling the export when JSON filetype is selected.
    /// </summary>
    /// <param name="tasks"></param>
    /// <exception cref="NotImplementedException"></exception>
    private void ExportAsJson(List<ExportableTaskDTO> tasks)
    {
        if (tasks.Count == 0)
        {
            // TODO: Replace with notification system
            MessageBox.Show("There are no tasks to export.");
            return;
        }

        var saveFileDialog = new SaveFileDialog
        {
            Filter = "JSON files (*.json)|*.json",
            FileName = "Tasks.json",
            Title = "Save Tasks as JSON"
        };

        if (saveFileDialog.ShowDialog() == true)
        {
            string filePath = saveFileDialog.FileName;

            try
            {
                var options = new JsonSerializerOptions { WriteIndented = true };
                string jsonString = JsonSerializer.Serialize(tasks, options);
                
                File.WriteAllText(filePath, jsonString);
                
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error exporting tasks as Json: {e.Message}");
            }
        }
    }
    
    #endregion
}