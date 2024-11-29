using System.Globalization;
using System.Windows.Input;
using WPF_MVVM_TEMPLATE.Application;
using WPF_MVVM_TEMPLATE.Application.Utility.Validation;
using WPF_MVVM_TEMPLATE.Entitys.DTOs;
using WPF_MVVM_TEMPLATE.Infrastructure;

namespace WPF_MVVM_TEMPLATE.Presentation.ViewModel;

public class LoginViewModel : ViewModelBase
{
    private readonly ValidationManager _validationManager = new();

    public LoginViewModel()
    {
        _validationManager.RegisterField(TbUsername);
        _validationManager.RegisterField(TbPassword);
    }

    public ICommand LoginCommand => new CommandBase(LoginLogic);

    private async void LoginLogic(Object o)
    {
        LoginRequestDTO loginRequestDto = new();
        loginRequestDto.username = TbUsername;
        loginRequestDto.password = TbPassword;
        
        // TODO: Debugging
        Console.WriteLine(loginRequestDto.username);
        Console.WriteLine(loginRequestDto.password);
        
        try
        {
            var webService = new WebService("http://localhost:8080/");
            var loginRepoApi = new UserRepoApi(webService);
            var login = new LoginAdmin(loginRepoApi);

            var loginResponse = await login.GetLoginRequest(loginRequestDto);
            
            Console.WriteLine(loginResponse?.ResponseBody);
            Console.WriteLine(loginResponse?.StatusCode);
            Console.WriteLine(loginResponse?.Headers);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.StackTrace);
        }

        
    }

    #region Getter & Setter

    private string _tbUsername;

    public string TbUsername
    {
        get { return _tbUsername; }
        set 
        { 
            _tbUsername = value; 
            OnPropertyChanged(); 
            
            var validationResult = new LettersAndSpacesRule().Validate(value, CultureInfo.CurrentCulture);

            if (validationResult.IsValid)
            {
                _validationManager.ClearErrors(nameof(TbUsername));
            }
            else
            {
                _validationManager.AddError(nameof(TbUsername), validationResult.ErrorContent.ToString());
            }
        }
    }
    
    private string _tbPassword;

    public string TbPassword
    {
        get { return _tbPassword; }
        set
        {
            _tbPassword = value;
            OnPropertyChanged();
            
            var validationResult = new LettersAndSpacesRule().Validate(value, CultureInfo.CurrentCulture);

            if (validationResult.IsValid)
            {
                _validationManager.ClearErrors(nameof(TbPassword));
            }
            else
            {
                _validationManager.AddError(nameof(TbPassword), validationResult.ErrorContent.ToString());
            }
        }
    }

    #endregion
    
}