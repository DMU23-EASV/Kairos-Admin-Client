using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.DirectoryServices;
using System.Windows.Input;
using Microsoft.Xaml.Behaviors.Core;
using WPF_MVVM_TEMPLATE.Application;
using WPF_MVVM_TEMPLATE.DTO;
using WPF_MVVM_TEMPLATE.Infrastructure;

namespace WPF_MVVM_TEMPLATE.Presentation.ViewModel;

public class ManageUserViewModel : ViewModelBase
{
    public ObservableCollection<ManageUserDTO> Users
    {
        get { return _users; }
        set
        {
            _users = value;
            OnPropertyChanged();
        }
    }
    private ObservableCollection<ManageUserDTO> _users;
    
    public string SearchText { get; set; }

    private string _searchText
    {
        get { return SearchText; }
        set {
            SearchText = value; 
            OnPropertyChanged();
            
        }
    }

    public ManageUserViewModel()
    {
        _users = new ObservableCollection<ManageUserDTO>();
        LoadUsers();
    }

    private async void LoadUsers()
    {

        var webService = new WebService("http://localhost:8080/");
        var userRepoApi = new UserRepoApi(webService);
        var loadUsers = new LoadUsers(userRepoApi);

        _users.Clear();
        var users = await loadUsers.GetUsers();

        // If no useres is returnd. return.
        if (users == null || users.Count <= 0) return;

        // Adding useres to the obs. collection.
        users.ForEach(user =>
        {
            if (user != null) _users.Add(user);
        });
    }

    private ObservableCollection<T> SortUsersBySearch<T>(string searchText, ObservableCollection<T> collection )
    {
        
        // We dont search if the string is empty
        if (string.IsNullOrWhiteSpace(searchText)) return collection;
        
        // if collection length is 0 or 1 just return it
        if (collection.Count <= 1) return collection;
        
        // Sorting collection by search text, then by the items alfa numeric value.
        var sortedItems = collection
            .Where(item => item != null)
            .OrderBy(item => item.ToString().Contains(searchText, StringComparison.OrdinalIgnoreCase) ? 0 : 1)
            .ThenBy(item => item.ToString())
            .ToList();
        
        return new ObservableCollection<T>(sortedItems);
        
    }
    
    
    
    
    
    
    #region Commands
    public ICommand LoadUsersCommand => new CommandBase(obj => LoadUsers());
    public ICommand CreateNewUserCommand => new CommandBase(obj => ViewModelController.Instance.SetCurrentViewModel<CreateUserViewModel>());
    public ICommand SortByUsernameAscending => new CommandBase(obj => Users = new ObservableCollection<ManageUserDTO>(Users.OrderBy(user => user.Username)));
    public ICommand SortByUsernameDepending => new CommandBase(obj => Users = new ObservableCollection<ManageUserDTO>(Users.OrderByDescending(user => user.Username)));
    public ICommand SortBySearchTextCommand => new CommandBase(obj => Users = new ObservableCollection<ManageUserDTO>(SortUsersBySearch<ManageUserDTO>(SearchText, Users)));
    
    #endregion Commands

}

