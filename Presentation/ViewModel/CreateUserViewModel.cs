using System.Windows;
using System.Windows.Input;
using WPF_MVVM_TEMPLATE.Entitys;

namespace WPF_MVVM_TEMPLATE.Presentation.ViewModel;

public class CreateUserViewModel : ViewModelBase
{
    private readonly Dictionary<string, string> _fields = new()
    {
        { "TextBoxFirstName", "" },
        { "TextBoxLastName", "" },
        { "TextBoxEmail", "" },
        { "TextBoxPhoneCode", "" },
        { "TextBoxPhoneNumber", "" },
        { "TextBoxUsername", "" },
        { "TextBoxPassword1", "" },
        { "TextBoxPassword2", "" },
        { "TextBoxRole", "" },
        { "TextBoxStatus", "" },
        { "TextBoxDepartment", "" },
        { "TextBoxComment", "" }
    };
    
    public string this[string field]
    {
        get => _fields.ContainsKey(field) ? _fields[field] : "";
        set
        {
            if (_fields.ContainsKey(field) && _fields[field] != value)
            {
                _fields[field] = value;
                OnPropertyChanged(field);
            }
        }
    }

    private void SavePerson(object obj)
    {
        var emptyFields = _fields.Where(field => string.IsNullOrWhiteSpace(field.Value))
            .Select(field => field.Key)
            .ToList();

        if (emptyFields.Any())
        {
            Console.WriteLine($"The following fields are empty: {string.Join(", ", emptyFields)}");
        }
        else
        {
            Console.WriteLine("All fields are valid!");
            // Perform save logic
        }
    }

    public ICommand SavePersonCommand => new CommandBase(SavePerson);
    
    
    
}