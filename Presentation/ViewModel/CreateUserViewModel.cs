using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Xml.Linq;
using WPF_MVVM_TEMPLATE.Application;
using WPF_MVVM_TEMPLATE.Application.Utility;
using WPF_MVVM_TEMPLATE.Application.Utility.Validation;
using WPF_MVVM_TEMPLATE.Entitys;
using WPF_MVVM_TEMPLATE.Entitys.DTOs;
using WPF_MVVM_TEMPLATE.Entitys.Enum;
using WPF_MVVM_TEMPLATE.Infrastructure;
using WPF_MVVM_TEMPLATE.Presentation.Service;

namespace WPF_MVVM_TEMPLATE.Presentation.ViewModel;
/*
 * View : CreateUserView
 * CreateUserViewModel Contains logic for the form used for creation of new users.
 * It handles validation on the user side and sends a User onwards to be handled by the API.
 */
public class CreateUserViewModel : ViewModelBase, INotifyDataErrorInfo
{
    private readonly ValidationManager _validationManager = new();
    
    public List<string> DialCodes { get; set; }
    // path to the "XMLFiles" folder where the XML file are stored
    private readonly string _directoryPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\FileResources\XMLFiles");
    


    
    // List of all text box property names
    private readonly List<string> _textBoxes = new List<string>
    {
        "TextBoxFirstName",
        "TextBoxLastName",
        "TextBoxEmail",
        "SelectedPhoneCode",
        "TextBoxPhoneNumber",
        "TextBoxUsername",
        "TextBoxPassword1",
        "TextBoxPassword2",
        "TextBoxDepartment",
    };
    
    //Default Error Message, we want to return something sensible.
    private string _defaultError = "Field is required";
    public bool IsSubmitButtonEnabled => !_validationManager.HasErrors;
    
    
    
    public CreateUserViewModel()
    {
        
        LoadDialCodes();
        
        _validationManager.RegisterFields(_textBoxes); //Ensure you have added your fields to the validation manager
        _validationManager.ErrorsChanged += (sender, args) => OnPropertyChanged(nameof(IsSubmitButtonEnabled));
        InitializeErrors(); // Call on startup to populate errors
        
    }

    private void LoadDialCodes()
    {
        try
        {
            var path = _directoryPath + "/landCodes.xml";
            var xmlDocument = XDocument.Load(path);
            

            // Uses LINQ to get all dial_codes
            DialCodes = (from landCode in xmlDocument.Descendants("landCode")
                select landCode.Element("dial_code")?.Value).ToList();
        
            CombBoxPhoneCodes = new ObservableCollection<string>(DialCodes);
            
        } catch (Exception e)
        {
            MessageBoxService.Instance.ShowMessageInfo("Noget gik galt, prøv igen", "Fejl", MessageBoxButton.OK, MessageBoxImage.Error);
            Console.WriteLine($"An error occur while loading Phone Codes {e}");
        }
        
    }
    
    //ICommand for Button CreateUser.
    public ICommand CreateUserCommand => new CommandBase(CreateUserLogic);
    
    /// <summary>
    /// Handles the logic for creating a user by gathering input data,
    /// sending it to the API, and handling the response.
    /// </summary>
    /// <param name="o">Optional parameter passed by the caller (unused).</param>
    private async void CreateUserLogic(object o)
    {
        try
        {
            CreateUserDTO userDTO = new()
            {
                username = TextBoxUsername,      
                password = TextBoxPassword2,    
                firstName = TextBoxFirstName,   
                lastName = TextBoxLastName,     
                phoneNumber = Convert.ToInt32(TextBoxPhoneNumber), 
                PhoneNumberLandCode = SelectedPhoneCode.Replace("+",""),  
                email = TextBoxEmail,           
                status = Convert.ToInt32(_selectedStatus), 
                department = TextBoxDepartment, 
                comment = TextBoxComment,       
                role = Convert.ToInt32(_selectedRole) 
            };

            var userRepo = new UserRepoApi(WebService.GetInstance("http://localhost:8080"));
            var createdUser =  new CreateUser(userRepo);
            var result = await createdUser.CreateUserAsync(userDTO);

            if (createdUser != null)
            {
                Console.WriteLine($"User {createdUser} created successfully!");
                
                // TODO: clear view? or change view??
            }
            else
            {
                Console.WriteLine("User creation failed. No response received.");
                MessageBoxService.Instance.ShowMessageInfo("Kunne ikke oprette medarbejderen", "Information", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        catch (Exception ex)
        {
            MessageBoxService.Instance.ShowMessageInfo("Noget gik galt!\nPrøv venligst igen", "Information", MessageBoxButton.OK, MessageBoxImage.Error);
            Console.WriteLine($"An error occurred while creating the user: {ex.Message}");
        }
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
    private string _selectedPhoneCode;
    public string SelectedPhoneCode
    {
        get { return _selectedPhoneCode; }
        set
        {
            if (_selectedPhoneCode != value)
            {
                _selectedPhoneCode = value;
                OnPropertyChanged(); 
                
                if (string.IsNullOrWhiteSpace(value))
                {
                    _validationManager.AddError(nameof(SelectedPhoneCode), "Landekode er påkrævet.");
                }
                else
                {
                    _validationManager.ClearErrors(nameof(SelectedPhoneCode));
                }
                
            }
        }
    }

    private ObservableCollection<string> _combBoxPhoneCodes = new ObservableCollection<string>
    {
       
    };

    public ObservableCollection<string> CombBoxPhoneCodes
    {
        get { return _combBoxPhoneCodes; }

        set
        {
            _combBoxPhoneCodes = value;
            OnPropertyChanged();
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
                _validationManager.AddError(nameof(TextBoxUsername), _defaultError);
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
                _validationManager.AddError(nameof(TextBoxPassword1), _defaultError);
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
                _validationManager.AddError(nameof(TextBoxPassword2), _defaultError);
            }
        }
        
    }

    private ObservableCollection<EUserRoles> _combBoxRole = new ObservableCollection<EUserRoles>
    {
        EUserRoles.Admin,
        EUserRoles.Bruger
    };

    public ObservableCollection<EUserRoles> CombBoxRole
    {
        get { return _combBoxRole; }
        set
        {
            _combBoxRole = value;

            OnPropertyChanged();
        }
    }

    private EUserRoles _selectedRole = EUserRoles.Bruger;
    public EUserRoles SelectedRole
    {
        get { return _selectedRole; }

        set
        {
            if (_selectedRole != value)
            {
                _selectedRole = value;
                OnPropertyChanged();
            }
        }
    }
    
    private ObservableCollection<EUserStatus> _combBoxStatus = new ObservableCollection<EUserStatus>
    { 
        EUserStatus.Aktiv,
        EUserStatus.Inaktiv,
    };
    
    public ObservableCollection<EUserStatus> CombBoxStatus
    {
        get { return _combBoxStatus; }
        set
        {
            _combBoxStatus = value;
            OnPropertyChanged();
        }
    }

    private EUserStatus _selectedStatus;
    public EUserStatus SelectedStatus
    {
        get { return _selectedStatus; }

        set
        {
            if (_selectedStatus != value)
            {
                _selectedStatus = value;
                OnPropertyChanged(); 
            }
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