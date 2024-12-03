using WPF_MVVM_TEMPLATE.Entitys;
using WPF_MVVM_TEMPLATE.Entitys.DTOs;

namespace WPF_MVVM_TEMPLATE.Application.DTOMapper;

public static class UserMapper
{
    //Returns FullUserDTO without Password.
    public static FullUserDTO MapToFullUserDto(User user)
    {
        return new FullUserDTO
        {
            Id = Convert.ToInt32(user.Id),
            username = user.Username,
            firstName = user.FirstName,
            lastName = user.LastName,
            phoneNumber = Convert.ToInt32(user.PhoneNumber),
            phoneNumberLandCode = user.PhoneNumberLandCode,
            email = user.Email,
            role = Convert.ToInt32(user.Role),
            status = Convert.ToInt32(user.Status),
            department = user.Department,
            comment = user.Comment
        };
    }
}