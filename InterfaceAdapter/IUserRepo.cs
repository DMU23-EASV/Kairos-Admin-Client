using WPF_MVVM_TEMPLATE.DTO;

namespace WPF_MVVM_TEMPLATE.InterfaceAdapter;

public interface IUserRepo
{
    Task<List<ManageUserDTO?>> GetUsers();
}