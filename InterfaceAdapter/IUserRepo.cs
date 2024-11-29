using WPF_MVVM_TEMPLATE.DTO;
using WPF_MVVM_TEMPLATE.Entitys;
using WPF_MVVM_TEMPLATE.Entitys.DTOs;

namespace WPF_MVVM_TEMPLATE.InterfaceAdapter;

public interface IUserRepo
{
    Task<List<ManageUserDTO?>> GetUsers();
    Task<ResponsPackage?> Login(LoginRequestDTO loginRequest);
}