using System.Globalization;
using System.Net;
using System.Net.Http.Headers;
using System.Windows;
using System.Windows.Input;
using WPF_MVVM_TEMPLATE.Application;
using WPF_MVVM_TEMPLATE.Application.Utility.Validation;
using WPF_MVVM_TEMPLATE.Entitys.DTOs;
using WPF_MVVM_TEMPLATE.Infrastructure;
using WPF_MVVM_TEMPLATE.Presentation.Service;

namespace WPF_MVVM_TEMPLATE.Presentation.ViewModel;

public class LoginViewModel : ViewModelBase
{
    private readonly ValidationManager _validationManager = new();

    public LoginViewModel()
    {
        _validationManager.RegisterField(nameof(TbUsername));
        _validationManager.RegisterField(nameof(TbPassword));
    }

    public ICommand LoginCommand => new CommandBase(LoginLogic);

    private async void LoginLogic(Object o)
    {
        LoginRequestDTO loginRequestDto = new();
        loginRequestDto.username = TbUsername;
        loginRequestDto.password = TbPassword;
        
        
        try
        {
            var webService = WebService.GetInstance("http://localhost:8080/");
            var loginRepoApi = new UserRepoApi(webService);
            var login = new LoginAdmin(loginRepoApi);

            var loginResponse = await login.GetLoginRequest(loginRequestDto);

            if (loginResponse.StatusCode != HttpStatusCode.OK)
            {
                MessageBoxService.Instance.ShowMessageInfo("Login Mislykkedes.", "Login Mislykkedes", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
            HttpHeaders headers = loginResponse.Headers;
            IEnumerable<string> values;

            if (headers.Contains("Set-Cookie"))
            {
                values = headers.GetValues("Set-Cookie");

                string[] subString = values.First().Split(";");
                
                string[] cookieValue = subString[0].Split("=");
                
                webService.AuthenticationHeader = cookieValue[1];
            }

            TbUsername = String.Empty;
            TbPassword = String.Empty;
            
            ViewModelController.Instance.SetCurrentViewModel<HomeViewModel>();
            
        }
        catch (Exception e)
        {
            Console.WriteLine(e.StackTrace);
        }

        
    }

    #region Getter & Setter

    private string? _tbUsername;

    public string? TbUsername
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
                _validationManager.AddError(nameof(TbUsername), validationResult.ErrorContent?.ToString() ?? "Unknown error");
            }
        }
    }
    
    private string? _tbPassword;

    public string? TbPassword
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
                _validationManager.AddError(nameof(TbPassword), validationResult.ErrorContent?.ToString() ?? "Unknown error");
            }
        }
    }

    #endregion
    
}