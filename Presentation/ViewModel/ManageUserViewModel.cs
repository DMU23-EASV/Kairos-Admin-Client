﻿using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using WPF_MVVM_TEMPLATE.Application;
using WPF_MVVM_TEMPLATE.DTO;
using WPF_MVVM_TEMPLATE.Infrastructure;
using WPF_MVVM_TEMPLATE.Presentation.Service;

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
    
    public Object SelectedObject {
        get { return _selectedObject; }
        set
        {
            _selectedObject = value;
            OnItemSelect(value);
            Console.WriteLine(_selectedObject);
        }
    }
    private Object _selectedObject { get; set; }
    

    public ManageUserViewModel()
    {
        _users = new ObservableCollection<ManageUserDTO>();
        LoadUsers();
    }

    
    /// <summary>
    /// Method for loading useres from API endpoint into OBS. collection.
    /// </summary>
    private async void LoadUsers()
    {

        var webService = WebService.GetInstance("http://localhost:8080/");
        var userRepoApi = new UserRepoApi(webService);
        var loadUsers = new LoadUsers(userRepoApi);

        _users.Clear();
        var users = await loadUsers.GetUsers();
        
        Console.WriteLine("Got all users");
        foreach (var manageUserDto in users)
        {
            Console.WriteLine(manageUserDto.Username);
        }

        // If no users is returned. return.
        if (users == null || users.Count <= 0)
        {
            MessageBoxService.Instance.ShowMessageInfo("Medarbejderne blev ikke fundet", "Fejl", MessageBoxButton.OK, MessageBoxImage.Information);
            Console.WriteLine("Users not found");
            return;
        }

        // Adding useres to the obs. collection.
        users.ForEach(user =>
        {
            if (user != null) _users.Add(user);
        });
    }
    
    /// <summary>
    /// Method for sorting an array by search text, Items witch does not contain the
    /// search text will be ordered alphanumerically. 
    /// </summary>
    /// <param name="searchText"> string searchtext</param>
    /// <param name="collection"> collection to sort.</param>
    /// <typeparam name="T">collection item source type</typeparam>
    /// <returns>sorted collection</returns>
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
    
    /// <summary>
    /// Handles the selection of an item and transitions to the Edit User view model 
    /// while transferring the selected item's data.
    /// </summary>
    /// <param name="item">The selected item, expected to be of type <see cref="ManageUserDTO"/>.</param>
    public void OnItemSelect(Object item)
    {
        Console.WriteLine($"OnItemSelect {item}");
        
        // Ensuring the item is of type ManageUserDTO
        if (item is not ManageUserDTO || SelectedObject == null)
        {
            Console.WriteLine("Selected item is Not of type ManageUserDTO");
            return;
        }
        
        // Transfering data to edit user and chaning view. 
        // TODO: IMPLEMENT. 
        var vm = ViewModelController.Instance.GetAllViewModels()[typeof(EditUserViewModel)];
        var vm2 = vm as EditUserViewModel;
        if (vm2 == null)
        {
            Console.WriteLine("Selected item is Not of type EditUserViewModel");
            return;
        }
        Console.WriteLine($"Selected Item: {vm2}");
        LoadUsers(); //Refresh the users view to clear selected.
        ViewModelController.Instance.SetCurrentViewModel<EditUserViewModel>();

        vm2.LoadUser(item as ManageUserDTO);
        
        Console.WriteLine("OnItemSelect");
    }
    
    #region Commands
    public ICommand LoadUsersCommand => new CommandBase(obj => LoadUsers());
    public ICommand CreateNewUserCommand => new CommandBase(obj => ViewModelController.Instance.SetCurrentViewModel<CreateUserViewModel>());
    public ICommand SortByUsernameAscending => new CommandBase(obj => Users = new ObservableCollection<ManageUserDTO>(Users.OrderBy(user => user.Username)));
    public ICommand SortByUsernameDepending => new CommandBase(obj => Users = new ObservableCollection<ManageUserDTO>(Users.OrderByDescending(user => user.Username)));
    public ICommand SortBySearchTextCommand => new CommandBase(obj => Users = new ObservableCollection<ManageUserDTO>(SortUsersBySearch<ManageUserDTO>(SearchText, Users)));
    
    #endregion Commands

}

