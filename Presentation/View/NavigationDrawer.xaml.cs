using System.Windows;
using System.Windows.Controls;
using WPF_MVVM_TEMPLATE.Presentation.ViewModel;

namespace WPF_MVVM_TEMPLATE.Presentation.View;

public partial class NavigationDrawer : UserControl
{
    public NavigationDrawer()
    {
        InitializeComponent();
    }

    private void SettingsNav_OnClick(object sender, RoutedEventArgs e)
    {
        ViewModelController.Instance.SetCurrentViewModel<SettingsViewModel>();
    }

    private void ExportDataNav_OnClick(object sender, RoutedEventArgs e)
    {
        ViewModelController.Instance.SetCurrentViewModel<ExportTaskViewModel>();
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
}