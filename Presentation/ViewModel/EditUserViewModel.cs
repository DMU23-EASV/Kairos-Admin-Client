using System.Collections;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Input;
using WPF_MVVM_TEMPLATE.Application;
using WPF_MVVM_TEMPLATE.Application.Utility.Validation;
using WPF_MVVM_TEMPLATE.DTO;
using WPF_MVVM_TEMPLATE.Entitys;
using WPF_MVVM_TEMPLATE.Entitys.DTOs;
using WPF_MVVM_TEMPLATE.Infrastructure;

namespace WPF_MVVM_TEMPLATE.Presentation.ViewModel;

public class EditUserViewModel : ViewModelBase, INotifyDataErrorInfo
{
    private readonly ValidationManager _validationManager = new();
    private bool _validation = true;
    private int userid;
    
    // List of all text box property names
    private readonly List<string> _textBoxes = new List<string>
    {
        "TextBoxFirstName",
        "TextBoxLastName",
        "TextBoxEmail",
        "TextBoxPhoneCode",
        "TextBoxPhoneNumber",
        "TextBoxRole",
        "TextBoxStatus",
        "TextBoxDepartment",
    };

    public async void LoadUser(ManageUserDTO user)
    {
        Console.WriteLine($"Loading User details {user}");
        _validation = false;
        FullUserDTO editUser;
        try
        {
            var userRepo = new UserRepoApi(WebService.GetInstance("http://localhost:8080"));
            var loadedUser =  new GetUserByUsername(userRepo);
            editUser = await loadedUser.GetUser(user.Username);
        }
        catch (Exception e)
        {
            MessageBox.Show(e.Message);
            ViewModelController.Instance.SetCurrentViewModel<ManageUserViewModel>();
            return;
        }

        userid = editUser.Id;
        TextBoxFirstName = editUser.firstName;
        TextBoxLastName = editUser.lastName;
        TextBoxUsername = editUser.username;
        TextBoxEmail = editUser.email;
        TextBoxPhoneNumber = editUser.phoneNumber.ToString();
        TextBoxPhoneCode = editUser.phoneNumberLandCode;
        TextBoxEmail = editUser.email;
        TextBoxRole = editUser.role.ToString();
        TextBoxStatus = editUser.status.ToString();
        TextBoxDepartment = editUser.department;
        TextBoxComment = editUser.comment;

        InitializeErrors(); // Call on startup to populate errors
        _validation = true;
        Console.WriteLine($"Loaded User details {editUser}");
    }
    
    //Default Error Message, we want to return something sensible.
    private string _defaultError = "Field is required";
    public bool IsSubmitButtonEnabled => !_validationManager.HasErrors;
    
    public EditUserViewModel()
    { 
        _validationManager.RegisterFields(_textBoxes); //Ensure you have added your fields to the validation manager
        _validationManager.ErrorsChanged += (sender, args) => OnPropertyChanged(nameof(IsSubmitButtonEnabled));
    }

    public ICommand EditUserCommand => new CommandBase(EditUserLogic);
    
    public void ClearFields()
    {
        _validation = false;
        
        userid = -1;
        TextBoxFirstName = string.Empty;
        TextBoxLastName = string.Empty;
        TextBoxEmail = string.Empty;
        TextBoxPhoneCode = string.Empty;
        TextBoxPhoneNumber = string.Empty;
        TextBoxUsername = string.Empty;
        TextBoxRole = string.Empty;
        TextBoxStatus = string.Empty;
        TextBoxDepartment = string.Empty;
        TextBoxComment = string.Empty;
        
        _validation = true;
    }
    private async void EditUserLogic(object obj)
    {
        FullUserDTO editUser = new()
        {
            Id = userid,
            username = TextBoxUsername,
            firstName = TextBoxFirstName,
            lastName = TextBoxLastName,
            phoneNumber = Convert.ToInt32(TextBoxPhoneNumber),
            phoneNumberLandCode = TextBoxPhoneCode,
            email = TextBoxEmail,
            role = Convert.ToInt32(TextBoxRole),
            status = Convert.ToInt32(TextBoxStatus),
            department = TextBoxDepartment,
            comment = TextBoxComment,
        };
        
        var userRepo = new UserRepoApi(WebService.GetInstance("http://localhost:8080"));
        var editUserUseCase =  new EditUser(userRepo);
        var result = await editUserUseCase.UpdateUserAsync(editUser);

        if (result != null)
        {
            ClearFields();
            Console.WriteLine($"User changed successfully!");
            ViewModelController.Instance.SetCurrentViewModel<ManageUserViewModel>();
        }
        else
        {
            Console.WriteLine($"Didn't work!");
        }
    }

    public ICommand UpdatePassword => new CommandBase(UpdatePasswordLogic);

    private async void UpdatePasswordLogic(object obj)
    {
        //TODO: Use-case 32
        Console.WriteLine("UpdatePasswordLogic Missing : TODO UC-32");
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
            if (_validation)
            {
                var validationResult = new LettersAndSpacesRule().Validate(value, CultureInfo.CurrentCulture);

                if (validationResult.IsValid)
                {
                    _validationManager.ClearErrors(nameof(TextBoxFirstName));
                }
                else
                {
                    _validationManager.AddError(nameof(TextBoxFirstName), validationResult.ErrorContent.ToString());
                }  
            } else
            {
                _validationManager.ClearErrors(nameof(TextBoxUsername));
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
            if (_validation)
            {
                var validationResult = new LettersAndSpacesRule().Validate(value, CultureInfo.CurrentCulture);

                if (validationResult.IsValid)
                {
                    _validationManager.ClearErrors(nameof(TextBoxLastName));
                }
                else
                {
                    _validationManager.AddError(nameof(TextBoxLastName), validationResult.ErrorContent.ToString());
                }                
            } else
            {
                _validationManager.ClearErrors(nameof(TextBoxUsername));
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

            if (_validation)
            {
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
            } else
            {
                _validationManager.ClearErrors(nameof(TextBoxUsername));
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

            if (_validation)
            {
                var validationResult = new NumbersOnlyRule().Validate(value, CultureInfo.CurrentCulture);

                if (validationResult.IsValid)
                {
                    _validationManager.ClearErrors(nameof(TextBoxPhoneCode));
                }
                else
                {
                    _validationManager.AddError(nameof(TextBoxPhoneCode), validationResult.ErrorContent.ToString());
                }
            } else
            {
                _validationManager.ClearErrors(nameof(TextBoxUsername));
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

            if (_validation)
            {
                var validationResult = new NumbersOnlyRule().Validate(value, CultureInfo.CurrentCulture);

                if (validationResult.IsValid)
                {
                    _validationManager.ClearErrors(nameof(TextBoxPhoneNumber));
                }
                else
                {
                    _validationManager.AddError(nameof(TextBoxPhoneNumber), validationResult.ErrorContent.ToString());
                }
            } else
            {
                _validationManager.ClearErrors(nameof(TextBoxUsername));
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

            if (_validation)
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    _validationManager.ClearErrors(nameof(TextBoxUsername));
                }
                else
                {
                    _validationManager.AddError(nameof(TextBoxUsername), _defaultError);
                }
            }
            else
            {
                _validationManager.ClearErrors(nameof(TextBoxUsername));
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

    private void InitializeErrors()
    {
        Console.WriteLine("Initializing Errors");
        foreach (var fieldName in _textBoxes)
        {
            // Use reflection to get the property value dynamically
            var property = GetType().GetProperty(fieldName);
            if (property != null)
            {
                var value = property.GetValue(this) as string;

                // Check if the value is null or whitespace
                if (string.IsNullOrWhiteSpace(value))
                {
                    _validationManager.AddError(fieldName, _defaultError);
                }
                else
                {
                    _validationManager.ClearErrors(fieldName); // Clear any existing errors if valid
                }
            }
        }
        Console.WriteLine("Errors Initialized");
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