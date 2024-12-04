using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using WPF_MVVM_TEMPLATE.Application;
using WPF_MVVM_TEMPLATE.Entitys;
using WPF_MVVM_TEMPLATE.Entitys.Enum;
using WPF_MVVM_TEMPLATE.Infrastructure;
using WPF_MVVM_TEMPLATE.InterfaceAdapter;

namespace WPF_MVVM_TEMPLATE.Presentation.ViewModel;

public class EditTaskViewModel : ViewModelBase
{
    
    private const string TaskEndpoint = "Task?id=";
    private TaskModel? _selectedTask;
    public TaskModel? SelectedTask  { get => _selectedTask; set { _selectedTask = value; OnPropertyChanged(); } }
    public ObservableCollection<TaskModel?>? TaskCollection { get; set; } = new ObservableCollection<TaskModel>();
    private IWebService _webService;
    private ITaskRepo _taskRepo;
    
    

    public EditTaskViewModel()
    {
        _webService = new WebService("http://localhost:8080/");
        _taskRepo = new TaskRepoApi(_webService);
        LoadAllTasks(_webService, _taskRepo);
    }
    
    /// <summary>
    /// Method for loading tasks into a collection from the api. 
    /// </summary>
    /// <param name="webService"></param>
    /// <param name="taskRepo"></param>
    private async void LoadAllTasks(IWebService webService, ITaskRepo taskRepo)
    {
        var tasks = await taskRepo.GetAllTasks();
        
        if (tasks != null)
        {
            if (TaskCollection == null)
            {
                TaskCollection = new ObservableCollection<TaskModel>(tasks);
            }
            else
            {
                TaskCollection?.Clear();
                foreach (var taskModel in tasks) TaskCollection?.Add(taskModel);
            }
            
        }
    }
    
    
    
    /// <summary>
    /// Method for handeling approvment of a task, Asserts that the task is not a draft. 
    /// </summary>
    /// <param name="task"></param>
    private void ApproveTask(object task)
    {
        
        var taskModel = ValidateObjectTaskModel(task);
        // Validating. 
        if (taskModel == null)
        {
            MessageBox.Show("Task is not valid");
            return;
        }

        if (taskModel.ModelStatus == ETaskModelStatus.Draft)
        {
            MessageBox.Show("Task is draft, and can therefor not be approved");
        }
        
        // finding the index of the task in task collection. 
        int? index = TaskCollection.IndexOf(taskModel);
        if (index == null)
        {
            MessageBox.Show("The task is not a part of the collection");
            return;
        }
        
        UpdateTaskStatus(taskModel, ETaskModelStatus.Approved, index.Value);
    }

    
    /// <summary>
    /// Method for handeling rejection of a task. Asserts the tasks is not a draft. 
    /// </summary>
    /// <param name="task"></param>
    private void RejectTask(object task)
    {
                
        var taskModel = ValidateObjectTaskModel(task);
        // Validating. 
        if (taskModel == null)
        {
            MessageBox.Show("Task is not valid");
            return;
        }

        if (taskModel.ModelStatus == ETaskModelStatus.Draft)
        {
            MessageBox.Show("Task is draft, and can not be rejected");
            return;
        }
        
        // finding the index of the task in task collection. 
        int? index = TaskCollection.IndexOf(taskModel);
        if (index == null)
        {
            MessageBox.Show("The task is not a part of the collection");
            return;
        }
        
        UpdateTaskStatus(taskModel, ETaskModelStatus.Rejected, index.Value);
    }

    /// <summary>
    /// Method for adding a comment to a task. 
    /// </summary>
    /// <param name="task"></param>
    private void AddCommentTask(object task)
    {
        Console.WriteLine("Task comment Added");
    }

    /// <summary>
    /// Method for validating a task.
    /// </summary>
    /// <param name="task"></param>
    /// <returns>null if obj passed is not of type Taskmodel, othervise returns object as taskmodel</returns>
    private static TaskModel? ValidateObjectTaskModel(object? task)
    {
        // Validating parameter. is not null or not type of Taskmodel.
        if (task == null || task is not TaskModel)
        {
            MessageBox.Show("Task is not a valid task");
            return null;
        }
        
        // casting task to task model obj.
        return task as TaskModel; 
    }
    
    /// <summary>
    /// Method for updating the modelstatus of a task
    /// </summary>
    /// <param name="task"></param>
    /// <param name="status"></param>
    /// <param name="index"></param>
    private async Task UpdateTaskStatus(TaskModel task, ETaskModelStatus status, int index)
    {
        
        // Checking if task is already approved. 
        if (task.ModelStatus == status) return;
        
        // marking task as approved. 
        task.ModelStatus = status;
        
        // updating the task in the collection. 
        TaskCollection.RemoveAt(index);
        TaskCollection.Insert(index, task);
        
        // updating the task on API. 
        var usecase = new UpdateTask(_taskRepo); 
        var url = $"{TaskEndpoint}{task.Id}";
        await usecase.UpdateTaskAsync(task, url);
        Console.WriteLine("Task Updated");

    }

    #region Collectionsorting
    
    /// <summary>
    /// Method for sorting a collection by task status.
    /// Sorts the collection injected as param. 
    /// </summary>
    /// <param name="status"></param>
    /// <param name="collection"></param>
    private void SortCollection(ETaskModelStatus status, ObservableCollection<TaskModel> collection)
    {
        
        // checking collection size. No need to sort if collection is null or of size 1.
        if (collection == null || collection.Count <= 1) return;
        
        var unSortedItems = TaskCollection.ToList();
        var sortedItems = collection.ToList().Where(item => item.ModelStatus == status).ToList();
        
        foreach (var item in sortedItems)
        {
            unSortedItems.Remove(item);
        }
        
        TaskCollection.Clear();
        sortedItems.ForEach(item => TaskCollection.Add(item));
        unSortedItems.ForEach(item => TaskCollection.Add(item));
        
    }
    
    private void SortCollectionByRejected(object obj)
    {
        SortCollection(ETaskModelStatus.Rejected, TaskCollection);
    }
    
    private void SortCollectionByApproved(object obj)
    {
        SortCollection(ETaskModelStatus.Approved, TaskCollection);
    }
    
    private void SortCollectionByAwaitingApproved(object obj)
    {
        SortCollection(ETaskModelStatus.AwaitingApproval, TaskCollection);
    }    
    private void SortCollectionByDraft(object obj)
    {
        SortCollection(ETaskModelStatus.Draft, TaskCollection);
    }
    
    
    
    
    

    #endregion
    
    # region Commands
    public ICommand SortByArchived => new CommandBase(SortCollectionByDraft);
    public ICommand SortByAwaitingApproval => new CommandBase(SortCollectionByAwaitingApproved);
    public ICommand SortByApproved => new CommandBase(SortCollectionByApproved);
    public ICommand SortByRejected => new CommandBase(SortCollectionByRejected);
    public ICommand RejectTaskCommand => new CommandBase(RejectTask);
    public ICommand AddTaskComment => new CommandBase(AddCommentTask);
    public ICommand ApproveTaskCommand => new CommandBase(ApproveTask);
    public ICommand LoadTaskCommand => new CommandBase(obj => LoadAllTasks(_webService, _taskRepo));
    
    #endregion

}