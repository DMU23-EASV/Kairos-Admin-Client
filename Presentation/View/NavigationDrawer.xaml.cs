using System.Runtime.InteropServices.JavaScript;
using System.Windows;
using System.Windows.Controls;
using WPF_MVVM_TEMPLATE.Infrastructure;
using WPF_MVVM_TEMPLATE.Presentation.ViewModel;

namespace WPF_MVVM_TEMPLATE.Presentation.View;

public partial class NavigationDrawer : UserControl
{
    public NavigationDrawer()
    {
        InitializeComponent();
    }

    private async void Logout_OnClick(object sender, RoutedEventArgs e)
    {
        var userRepo = new UserRepoApi(WebService.GetInstance("http://localhost:8080"));
        var response = await userRepo.Logout();
        
        // In case logout falis, send error.
        if (response.StatusCode != System.Net.HttpStatusCode.OK)
        {
            // Send error here
            return;
        }

        // In case successful logout, send user to login view
        if (response.StatusCode == System.Net.HttpStatusCode.OK)
        {
            
            ViewModelController.Instance.SetCurrentViewModel<LoginViewModel>();
        }
            
            
    }

    private void SettingsNav_OnClick(object sender, RoutedEventArgs e)
    {
        ViewModelController.Instance.SetCurrentViewModel<SettingsViewModel>();
    }

    private void HomeNav_OnClick(object sender, RoutedEventArgs e)
    {
        ViewModelController.Instance.SetCurrentViewModel<HomeViewModel>();
    }

    private void CreateUserNav_OnClick(object sender, RoutedEventArgs e)
    {
        ViewModelController.Instance.SetCurrentViewModel<CreateUserViewModel>();
    }

    private void ManageUSersNav_OnClick(object sender, RoutedEventArgs e)
    {
        ViewModelController.Instance.SetCurrentViewModel<ManageUserViewModel>();
    }
    
    private void EditUserNav_OnClick(object sender, RoutedEventArgs e)
    {
        ViewModelController.Instance.SetCurrentViewModel<EditUserViewModel>();
    }

    private void EditUserNav_Onclick(object sender, RoutedEventArgs e)
    {
        ViewModelController.Instance.SetCurrentViewModel<EditTaskViewModel>();
    }
}