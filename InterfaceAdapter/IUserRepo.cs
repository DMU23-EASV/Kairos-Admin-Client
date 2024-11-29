using WPF_MVVM_TEMPLATE.DTO;
using WPF_MVVM_TEMPLATE.Entitys.DTOs;

namespace WPF_MVVM_TEMPLATE.InterfaceAdapter;

public interface IUserRepo
{
    Task<List<ManageUserDTO?>> GetUsers();

    Task<CreateUserDTO?> CreateUser(CreateUserDTO user);
    Task<FullUserDTO?> GetUserByEmail(string email);
    Task<FullUserDTO?> GetUserByUsername(string username);
}