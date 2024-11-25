using System.Windows;
using System.Windows.Input;
using WPF_MVVM_TEMPLATE.Entitys;

namespace WPF_MVVM_TEMPLATE.Presentation.ViewModel;
/*
 * CreateUserViewModel Contains the form for creation of new users.
 * We use a dictionary to hold all the textboxes, this is to ensure we dont repeat our selves more than needed.
 * 
 */
public class CreateUserViewModel : ViewModelBase
{
    //Our Dictionary with all our Textboxes, initialized to get a valid bind.
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
    
    //Our generalized getter and setter for our fields. We validate through our dictionary
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

    //Our savePerson method, we check to see if any fields are empty and if they are we save them to keep track of which.
    private void SavePerson(object obj)
    {
        var emptyFields = _fields.Where(field => string.IsNullOrWhiteSpace(field.Value))
            .Select(field => field.Key)
            .ToList();

        if (emptyFields.Any())
        {
            Console.WriteLine($"The following fields are empty :D {string.Join(", ", emptyFields)}");
        }
        else
        {
            Console.WriteLine("All fields are valid! :-)");
            // Perform save logic
        }
    }
    //Our ICommand, required to make connection between View and ViewModel 
    public ICommand SavePersonCommand => new CommandBase(SavePerson);
    
}