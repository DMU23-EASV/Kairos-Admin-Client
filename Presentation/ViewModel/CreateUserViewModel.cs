using System.Windows;
using System.Windows.Input;
using WPF_MVVM_TEMPLATE.Entitys;

namespace WPF_MVVM_TEMPLATE.Presentation.ViewModel;

public class CreateUserViewModel : ViewModelBase
{
    private string _username;
    
  

    public string TextBoxUsername
    {
        get => _username;
        set
        {
            if (_username != value)
            {
                _username = value;
                OnPropertyChanged(nameof(TextBoxUsername));
            }
        }
    }

    private void SavePerson(object obj)
    {
        if (!string.IsNullOrWhiteSpace(TextBoxUsername))
        {
            Console.Out.WriteLine($"IM A PRETEND OBJECT :-D - Username: {TextBoxUsername}");
        }
        else
        {
            Console.Out.WriteLine($"Username is empty");
        }
    }

    public ICommand SavePersonCommand => new CommandBase(SavePerson);
    
    
    
}