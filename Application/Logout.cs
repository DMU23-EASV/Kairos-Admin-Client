using WPF_MVVM_TEMPLATE.Entitys;
using WPF_MVVM_TEMPLATE.InterfaceAdapter;

namespace WPF_MVVM_TEMPLATE.Application;

public class Logout
{
    private readonly IUserRepo _user;

    public Logout(IUserRepo userRepo)
    {
        _user = userRepo;
    }
    
    public async Task<ResponsPackage?> LogoutTask() => await _user.Logout();
}