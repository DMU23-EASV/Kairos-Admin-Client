using System.Windows;
using System.Windows.Controls;
using WPF_MVVM_TEMPLATE.Presentation.ViewModel;

namespace WPF_MVVM_TEMPLATE.Presentation.View;

public partial class HomeView : UserControl
{
    public HomeView()
    {
        InitializeComponent();
    }

    private void NavigateToManageTasks(object sender, RoutedEventArgs e)
    {
        ViewModelController.Instance.SetCurrentViewModel<EditTaskViewModel>();
    }
}