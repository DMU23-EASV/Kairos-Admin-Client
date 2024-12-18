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

            //Console.WriteLine(loginResponse?.ResponseBody);
            //Console.WriteLine(loginResponse?.StatusCode);
            Console.WriteLine(loginResponse?.Headers);
            
            HttpHeaders headers = loginResponse.Headers;
            IEnumerable<string> values;
            if (headers.TryGetValues("Set-Cookie", out values))
            {
                Console.WriteLine("Please være cookie: " + values);
                webService.AuthenticationHeader = values.First();
            }
            else
            {
                Console.WriteLine("NOT FOUND");
            }
            
            

            TbUsername = String.Empty;
            TbPassword = String.Empty;

            webService.AuthenticationHeader = ("CfDJ8L7C_ebAHjxIiKfmXr8uSyHJJ97TgLbX50KkQGJ282m7QRCEtczHBLV-nNS2Rqy7tPK2YnE4lfqrqIA1oI-IZuOSFfz29ADEE9lOsF8PZ10ou11L3G9tUcYspZjLKuV_CnOyh-przvzhC3nBorgw8P52ulmzdENomOxesug9Y5irHsBlrd7kVZnDUqJbypS4AumiwRJ55SzfAkVXKNsSHV4vTxvMMjM3C1i9ueoxclo3VrDmSP35dfXofAPXwnr_ksyQxZBWIAHD_UDAFO-XMVL1lQVm9rwnGF7DzOr6j7WBz_v-sjFY47VHoB_JqQ3cy8GELd_EQqiO-zNn44tnO-U");
            
            
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