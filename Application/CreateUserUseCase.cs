using WPF_MVVM_TEMPLATE.Entitys.DTOs;
using WPF_MVVM_TEMPLATE.InterfaceAdapter;

namespace WPF_MVVM_TEMPLATE.Application;

public class CreateUserUseCase
{
    private readonly IApiService _apiService;

    public CreateUserUseCase(IApiService apiService)
    {
        _apiService = apiService;
    }

    public async Task<CreateUserDTO?> CreateUser(CreateUserDTO user)
    {
        return await _apiService.CreateUserAsync(user);
    }
}