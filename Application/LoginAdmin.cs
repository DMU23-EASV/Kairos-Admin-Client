using WPF_MVVM_TEMPLATE.Entitys;
using WPF_MVVM_TEMPLATE.Entitys.DTOs;
using WPF_MVVM_TEMPLATE.InterfaceAdapter;

namespace WPF_MVVM_TEMPLATE.Application;

public class LoginAdmin
{
    private readonly IUserRepo _userRepo;

    public LoginAdmin(IUserRepo userRepo)
    {
        _userRepo = userRepo;
    }

    /// <summary>
    /// Method for logging into the system
    /// </summary>
    /// <returns>Returns response code if login fails, or a login successful message if successful.</returns>
    public async Task<ResponsPackage?> GetLoginRequest(LoginRequestDTO request) => await _userRepo.Login(request);
}