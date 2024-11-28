using WPF_MVVM_TEMPLATE.Entitys.DTOs;

namespace WPF_MVVM_TEMPLATE.InterfaceAdapter;

public interface IApiService
{
    Task<CreateUserDTO?> CreateUserAsync(CreateUserDTO user);
}