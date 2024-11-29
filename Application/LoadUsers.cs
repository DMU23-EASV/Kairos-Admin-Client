using WPF_MVVM_TEMPLATE.DTO;
using WPF_MVVM_TEMPLATE.InterfaceAdapter;

namespace WPF_MVVM_TEMPLATE.Application;

public class LoadUsers
{
    private readonly IUserRepo _userRepo;
    public LoadUsers(IUserRepo userRepo)
    {
        _userRepo = userRepo;
    }
    
    /// <summary>
    /// Method for loading all users
    /// </summary>
    /// <returns>Returns a list of users, or a empty list if no useres is found</returns>
    public async Task<List<ManageUserDTO?>> GetUsers() => await _userRepo.GetUsers();
    
}