using System.Collections;
using System.ComponentModel;
using System.Globalization;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WPF_MVVM_TEMPLATE.Application;
using WPF_MVVM_TEMPLATE.Application.Utility;
using WPF_MVVM_TEMPLATE.Entitys;
using WPF_MVVM_TEMPLATE.Entitys.DTOs;
using WPF_MVVM_TEMPLATE.Infrastructure;

namespace WPF_MVVM_TEMPLATE.Presentation.ViewModel;
/*
 * View : CreateUserView
 * CreateUserViewModel Contains logic for the form used for creation of new users.
 * It handles validation on the user side and sends a User onwards to be handled by the API.
 */
public class CreateUserViewModel : ViewModelBase, INotifyDataErrorInfo
{
    private readonly ValidationManager _validationManager = new();
    
    // List of all text box property names
    private List<string> _textBoxes = new List<string>
    {
        "TextBoxFirstName",
        "TextBoxLastName",
        "TextBoxEmail",
        "TextBoxPhoneCode",
        "TextBoxPhoneNumber",
        "TextBoxUsername",
        "TextBoxPassword1",
        "TextBoxPassword2",
        "TextBoxRole",
        "TextBoxStatus",
        "TextBoxDepartment",
        "TextBoxComment"
    };
    
    
    private string defaultError = "Field is required";
    public bool IsSubmitButtonEnabled => !_validationManager.HasErrors;
    
    
    
    public CreateUserViewModel()
    { 
        _validationManager.RegisterFields(_textBoxes); //Ensure you have added your fields to the validation manager
        _validationManager.ErrorsChanged += (sender, args) => OnPropertyChanged(nameof(IsSubmitButtonEnabled));
        InitializeErrors(); // Call on startup to populate errors
    }
    public ICommand CreateUserCommand => new CommandBase(CreateUserLogic);
    private void CreateUserLogic(object o)
    {
        Console.WriteLine("HELLO WORLD??!?");
        CreateUserDTO userDTO = new();
        userDTO.username = TextBoxUsername;
        userDTO.password = TextBoxPassword2;
        userDTO.firstName = TextBoxFirstName;
        userDTO.lastName = TextBoxLastName;
        userDTO.phoneNumber = Convert.ToInt32(TextBoxPhoneNumber);
        userDTO.contryCode = TextBoxPhoneCode;
        userDTO.email = TextBoxEmail;
        userDTO.status = Convert.ToInt32(TextBoxStatus);
        userDTO.department = TextBoxDepartment;
        userDTO.comment = TextBoxComment;
        userDTO.role = Convert.ToInt32(TextBoxRole);
        
        CreateUserUseCase create = new CreateUserUseCase(new ApiService(new HttpClient()));
        
        //TODO: Handle response?
        Console.WriteLine(create.CreateUser(userDTO));
    }
    
    #region Getter & Setter
    
    private string _textBoxFirstName;
    public string TextBoxFirstName
    {
        get { return _textBoxFirstName; }
        set
        {
            _textBoxFirstName = value; 
            
            OnPropertyChanged();
            
            var validationResult = new LettersAndSpacesRule().Validate(value, CultureInfo.CurrentCulture);

            if (validationResult.IsValid)
            {
                _validationManager.ClearErrors(nameof(TextBoxFirstName));
            }
            else
            {
                _validationManager.AddError(nameof(TextBoxFirstName), validationResult.ErrorContent.ToString());
            }
        }
    }
    private string _textBoxLastName;
    public string TextBoxLastName
    {
        get { return _textBoxLastName; }
        set
        {
            _textBoxLastName = value;

            OnPropertyChanged();
            
            var validationResult = new LettersAndSpacesRule().Validate(value, CultureInfo.CurrentCulture);

            if (validationResult.IsValid)
            {
                _validationManager.ClearErrors(nameof(TextBoxLastName));
            }
            else
            {
                _validationManager.AddError(nameof(TextBoxLastName), validationResult.ErrorContent.ToString());
            }
        }
    }
    private string _textBoxEmail;
    public string TextBoxEmail
    {
        get { return _textBoxEmail; }
        set
        {
            // Log the new Email value to the console
            Console.WriteLine($"Setting TextBoxEmail to: {value}");

            _textBoxEmail = value;
            OnPropertyChanged();

            // Clear any previous errors
            Console.WriteLine("Clearing previous errors for TextBoxEmail.");
            _validationManager.ClearErrors(nameof(TextBoxEmail));

            // Perform validation (Email validation in this case)
            var validationResult = new EmailRule().Validate(value, CultureInfo.CurrentCulture);
            Console.WriteLine($"Validation result: IsValid = {validationResult.IsValid}, ErrorContent = {validationResult.ErrorContent}");

            if (!validationResult.IsValid)
            {
                // Add error if validation fails
                Console.WriteLine($"Adding error for TextBoxEmail: {validationResult.ErrorContent}"); 
                _validationManager.AddError(nameof(TextBoxEmail), validationResult.ErrorContent.ToString());            }
            else
            {
                // Ensure the error is cleared if the validation passes
                Console.WriteLine("Validation passed. No errors for TextBoxEmail.");
                _validationManager.ClearErrors(nameof(TextBoxEmail));
            }
        }
    }

    //TODO: Change to something else? Drop down box??
    private string _textBoxPhoneCode;
    public string TextBoxPhoneCode
    {
        get { return _textBoxPhoneCode; }
        set
        {
            _textBoxPhoneCode = value;

            OnPropertyChanged();
            var validationResult = new NumbersOnlyRule().Validate(value, CultureInfo.CurrentCulture);

            if (validationResult.IsValid)
            {
                _validationManager.ClearErrors(nameof(TextBoxPhoneCode));
            }
            else
            {
                _validationManager.AddError(nameof(TextBoxPhoneCode), validationResult.ErrorContent.ToString());
            }
            
        }
    }
    private string _textBoxPhoneNumber;
    public string TextBoxPhoneNumber
    {
        get { return _textBoxPhoneNumber; }
        set
        {
            _textBoxPhoneNumber = value;

            OnPropertyChanged();
            var validationResult = new NumbersOnlyRule().Validate(value, CultureInfo.CurrentCulture);

            if (validationResult.IsValid)
            {
                _validationManager.ClearErrors(nameof(TextBoxPhoneNumber));
            }
            else
            {
                _validationManager.AddError(nameof(TextBoxPhoneNumber), validationResult.ErrorContent.ToString());
            }
        }
    }
    private string _textBoxUsername;
    public string TextBoxUsername
    {
        get { return _textBoxUsername; }
        set
        {
            _textBoxUsername = value;

            OnPropertyChanged();

            if (!string.IsNullOrWhiteSpace(value))
            {
                _validationManager.ClearErrors(nameof(TextBoxUsername));
            }
            else
            {
                _validationManager.AddError(nameof(TextBoxUsername), defaultError);
            }
        }
    }

    private string _textBoxPassword1;
    public string TextBoxPassword1
    {
        get { return _textBoxPassword1; }
        set
        {
            _textBoxPassword1 = value;

            OnPropertyChanged();

            if (!string.IsNullOrWhiteSpace(value))
            {
                _validationManager.ClearErrors(nameof(TextBoxPassword1));
            }
            else
            {
                _validationManager.AddError(nameof(TextBoxPassword1), defaultError);
            }
            
        }
    }
    private string _textBoxPassword2;

    public string TextBoxPassword2
    {
        get { return _textBoxPassword2; }
        set
        {
            _textBoxPassword2 = value;

            OnPropertyChanged();
            if (!string.IsNullOrWhiteSpace(value) && TextBoxPassword1 == value)
            {
                _validationManager.ClearErrors(nameof(TextBoxPassword2));
            }
            else
            {
                _validationManager.AddError(nameof(TextBoxPassword2), defaultError);
            }
        }
        
    }
    private string _textBoxRole;

    public string TextBoxRole
    {
        get { return _textBoxRole; }
        set
        {
            _textBoxRole = value;

            OnPropertyChanged();
            _validationManager.ClearErrors(nameof(TextBoxRole));

        }
    }
    
    //TODO: NO VALIDATION, NEED TO KNOW WHAT WE WANT TO DO HERE
    private string _textBoxStatus;
    public string TextBoxStatus
    {
        get { return _textBoxStatus; }
        set
        {
            _textBoxStatus = value;

            OnPropertyChanged();
            _validationManager.ClearErrors(nameof(TextBoxStatus));

        }
    }
    
    //TODO: NO VALIDATION NEED TO KNOW WHAT TO DO HERE
    private string _textBoxDepartment;
    public string TextBoxDepartment
    {
        get { return _textBoxDepartment; }
        set
        {
            _textBoxDepartment = value;

            OnPropertyChanged();
            _validationManager.ClearErrors(nameof(TextBoxDepartment));

        }
    }
    //TODO: NO VALIDATION NEED TO KNOW WHAT TO DO HERE
    private string _textBoxComment;
    public string TextBoxComment
    {
        get { return _textBoxComment; }
        set
        {
            _textBoxComment = value;

            OnPropertyChanged();
            _validationManager.ClearErrors(nameof(TextBoxComment));

        }
    }
    /// <summary>
    /// On PropertyChanged is handled whenever the list is updated.
    /// </summary>
    
    #endregion
    
    #region Validation

    private void RegisterFields()
    {
        _validationManager.RegisterFields(new[]
        {
            nameof(TextBoxFirstName),
            nameof(TextBoxLastName),
            nameof(TextBoxEmail),
            nameof(TextBoxPhoneCode),
            nameof(TextBoxPhoneNumber),
            nameof(TextBoxUsername),
            nameof(TextBoxPassword1),
            nameof(TextBoxPassword2),
            nameof(TextBoxRole),
            nameof(TextBoxStatus),
            nameof(TextBoxDepartment),
            nameof(TextBoxComment)
        });
    }

    private void InitializeErrors()
    {
        foreach (var field in _textBoxes)
        {
            _validationManager.AddError(field, "Field is required.");
        }
    }

    public IEnumerable GetErrors(string propertyName)
    {
        return _validationManager.GetErrors(propertyName);
    }

    public bool HasErrors
    {
        get
        {
            return _validationManager.HasErrors;
        }
    }
    public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged
    {
        add => _validationManager.ErrorsChanged += value;
        remove => _validationManager.ErrorsChanged -= value;
    }
    
    #endregion
}