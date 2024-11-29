using WPF_MVVM_TEMPLATE.Entitys;
using WPF_MVVM_TEMPLATE.Entitys.DTOs;

namespace WPF_MVVM_TEMPLATE.Application.DTOMapper;

public static class UserMapper
{
    //Returns FullPerson without Password.
    //Example, probably useless on front-end
    public static FullUserDTO MapToFullUserDto(User user)
    {
        return new FullUserDTO
        {
            Id = user.Id,
            Email = user.Email,
            Role = user.Role,
            Status = user.Status,
            Username = user.Username,
            FirstName = user.FirstName,
            LastName = user.LastName,
            PhoneNumber = user.PhoneNumber,
            Department = user.Department,
            Comment = user.Comment
        };
    }
}