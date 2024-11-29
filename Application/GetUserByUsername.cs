using WPF_MVVM_TEMPLATE.DTO;
using WPF_MVVM_TEMPLATE.Entitys.DTOs;
using WPF_MVVM_TEMPLATE.InterfaceAdapter;

namespace WPF_MVVM_TEMPLATE.Application;

public class GetUserByUsername
{
    private readonly IUserRepo _userRepo;
    public GetUserByUsername(IUserRepo userRepo)
    {
        _userRepo = userRepo;
    }
    
    /// <summary>
    /// Method for loading all users
    /// </summary>
    /// <returns>Returns a list of users, or a empty list if no useres is found</returns>
    public async Task<FullUserDTO> GetUser(string username) => await _userRepo.GetUserByUsername(username) ?? throw new InvalidOperationException();
}