using WPF_MVVM_TEMPLATE.DTO;
using WPF_MVVM_TEMPLATE.Entitys.DTOs;
using WPF_MVVM_TEMPLATE.Entitys;
using WPF_MVVM_TEMPLATE.Entitys.DTOs;

namespace WPF_MVVM_TEMPLATE.InterfaceAdapter;

public interface IUserRepo
{
    Task<List<ManageUserDTO?>> GetUsers();
    Task<ResponsPackage?> Login(LoginRequestDTO loginRequest);

    Task<CreateUserDTO?> CreateUser(CreateUserDTO user);
    Task<FullUserDTO?> GetUserByEmail(string email);
    Task<FullUserDTO?> GetUserByUsername(string username);
}