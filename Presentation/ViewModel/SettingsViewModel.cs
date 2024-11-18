using System.Collections.ObjectModel;
using System.Windows.Input;

namespace WPF_MVVM_TEMPLATE.Presentation.ViewModel;

public class SettingsViewModel : ViewModelBase
{

    public ObservableCollection<string> Countries
    {
        get
        {
            return _countries; 
        }
        set
        {
            _countries = value;
            OnPropertyChanged();
        }
    } 
    private ObservableCollection<string> _countries = new ObservableCollection<string>
    {
        "Denmark",
        "Sweden",
        "Norway",
        "Finland",
        "Iceland"
    };


    public ICommand NavigateBackCommand => new CommandBase(NavigateBack);
    private void NavigateBack(object obj)
    {
        ViewModelController.Instance.SetCurrentViewModel<HomeViewModel>();
    }
}