﻿using System.Collections.ObjectModel;
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
    
    
    #region Commands
    public ICommand LoadUsersCommand => new CommandBase(obj => LoadUsers());
    public ICommand CreateNewUserCommand => new CommandBase(obj => Console.WriteLine("Create new user"));
    public ICommand SearchCommand => new CommandBase(obj => Console.WriteLine("Search user"));
    
    #endregion Commands

}
