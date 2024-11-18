using System.Windows;
using System.Windows.Controls;
using WPF_MVVM_TEMPLATE.Presentation.ViewModel;

namespace WPF_MVVM_TEMPLATE.Presentation.View;

public partial class TopBar : UserControl
{
    public TopBar()
    {
        InitializeComponent();
    }

    private void Navigate_Home(object sender, RoutedEventArgs e)
    {
        ViewModelController.Instance.SetCurrentViewModel<HomeViewModel>();
    }
}