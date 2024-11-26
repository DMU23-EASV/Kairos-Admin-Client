using WPF_MVVM_TEMPLATE.Entitys;
using WPF_MVVM_TEMPLATE.Entitys.DTOs;

namespace WPF_MVVM_TEMPLATE.Application.DTOMapper;

public static class PersonMapper
{
    //Returns FullPerson without Password.
    public static FullPersonDto MapToFullPersonDto(Person person)
    {
        return new FullPersonDto
        {
            Id = person.id,
            Email = person.email,
            Role = person.role,
            Status = person.status,
            Username = person.username,
            FirstName = person.firstName,
            LastName = person.lastName,
            PhoneNumber = person.phoneNumber,
            Department = person.department,
            Comment = person.comment
        };
    }
}