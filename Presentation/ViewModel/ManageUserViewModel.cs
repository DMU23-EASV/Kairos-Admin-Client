using System.Collections.ObjectModel;
using System.Windows.Input;
using Microsoft.Xaml.Behaviors.Core;
using WPF_MVVM_TEMPLATE.Application;
using WPF_MVVM_TEMPLATE.DTO;
using WPF_MVVM_TEMPLATE.Infrastructure;

namespace WPF_MVVM_TEMPLATE.Presentation.ViewModel;

public class ManageUserViewModel : ViewModelBase
{
    public ObservableCollection<ManageUserDTO> Users {
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
    private ObservableCollection<ManageUserDTO> _users;

    public ManageUserViewModel()
    {
        _users = new ObservableCollection<ManageUserDTO>();
        LoadUsers(); 
    }
    
    public async void LoadUsers()
    {
        var ws = new WebService("http://localhost:8080/");
        var lu = new LoadUsers(ws);
        Console.WriteLine("Loading users...");
        _users.Clear();
        var users = await lu.FetchUsers();
        
        // Validating respons from the API.
        if (users == null || users.Count <= 0) return; 
        
        users.ForEach(user =>
        {
            if (user != null) _users.Add(user);
        });
    }

    public ICommand AddUserCommand = new CommandBase(AddUser);
    private static void AddUser(object obj)
    {
        //ViewModelController.Instance.SetCurrentViewModel<CreateUser>();
    }
}