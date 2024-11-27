using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using WPF_MVVM_TEMPLATE.Entitys;

namespace WPF_MVVM_TEMPLATE.Presentation.ViewModel;
/*
 * View : CreateUserView
 * CreateUserViewModel Contains logic for the form used for creation of new users.
 * It handles validation on the user side and sends a Person onwards to be handled by the API.
 */
public class CreateUserViewModel : ViewModelBase
{
    #region Getter & Setter
    private string _TextBoxFirstName;
    public string TextBoxFirstName
    {
        get { return _TextBoxFirstName; }
        set
        {
            _TextBoxFirstName = value; 
            OnPropertyChanged();
        }
    }
    private string _TextBoxLastName;
    public string TextBoxLastName
    {
        get { return _TextBoxLastName; }
        set
        {
            _TextBoxLastName = value;
            OnPropertyChanged();
        }
    }
    private string _TextBoxEmail;
    public string TextBoxEmail
    {
        get { return _TextBoxEmail; }
        set
        {
            _TextBoxEmail = value;
            OnPropertyChanged();
        }
    }

    private string _TextBoxPhoneCode;
    public string TextBoxPhoneCode
    {
        get { return _TextBoxPhoneCode; }
        set
        {
            _TextBoxPhoneCode = value;
            OnPropertyChanged();
        }
    }
    private string _TextBoxPhoneNumber;
    public string TextBoxPhoneNumber
    {
        get { return _TextBoxPhoneNumber; }
        set
        {
            _TextBoxPhoneNumber = value;
            OnPropertyChanged();
        }
    }
    private string _TextBoxUsername;
    public string TextBoxUsername
    {
        get { return _TextBoxUsername; }
        set
        {
            _TextBoxUsername = value;
            OnPropertyChanged();
        }
    }

    private string _TextBoxPassword1;
    public string TextBoxPassword1
    {
        get { return _TextBoxPassword1; }
        set
        {
            _TextBoxPassword1 = value;
            OnPropertyChanged();
        }
    }
    private string _TextBoxPassword2;

    public string TextBoxPassword2
    {
        get { return _TextBoxPassword2; }
        set
        {
            _TextBoxPassword2 = value;
            OnPropertyChanged();
        }
    }
    private string _TextBoxRole;

    public string TextBoxRole
    {
        get { return _TextBoxRole; }
        set
        {
            _TextBoxRole = value;
            OnPropertyChanged();
        }
    }
    private string _TextBoxStatus;
    public string TextBoxStatus
    {
        get { return _TextBoxStatus; }
        set
        {
            _TextBoxStatus = value;
            OnPropertyChanged();
        }
    }
    private string _TextBoxDepartment;

    public string TextBoxDepartment
    {
        get { return _TextBoxDepartment; }
        set
        {
            _TextBoxDepartment = value;
            OnPropertyChanged();
        }
    }
    private string _TextBoxComment;

    public string TextBoxComment
    {
        get { return _TextBoxComment; }
        set
        {
            _TextBoxComment = value;
            OnPropertyChanged();
        }
    }
    #endregion
    
    #region Regex Validation
    /*
     * All of our validation use Regex for its flexiblity
     */
    private bool ValidateNumber(string value)
    {
        //Regex check for numbers only (0-9) 
        string pattern = @"^\d$";
        if (string.IsNullOrEmpty(value) || !Regex.IsMatch(value, pattern))
        {
            return false;
        }
        return true;
    }
    
    private bool ValidateEmail(string value)
    {
        //Regex check for 1 (@) and 1 OR more .
        string pattern = @"^[^@]+@[^@]+\.[^@]+$";
        if (string.IsNullOrEmpty(value) || !Regex.IsMatch(value, pattern))
        {
            return false;
        }
        return true;
    }
    
    private bool ValidateLettersAndSpaces(string value)
    {
        // Regex to allow only letters (A-Z, a-z) and spaces
        string pattern = @"^[a-zA-Z]+$";
        if (string.IsNullOrEmpty(value) || !Regex.IsMatch(value, pattern))
        {
            return false; 
        }
        return true; 
    }
    
    #endregion
    
    
    
}