namespace WPF_MVVM_TEMPLATE.Entitys.DTOs;
/*
 * A FullPerson Object that is API "safe" no password is exposed.
 */
public class FullPersonDto
{
    public string Id { get; set; }
    public string Email { get; set; }
    public string Role { get; set; }
    public string Status { get; set; }
    public string Username { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PhoneNumber { get; set; }
    public string Department { get; set; }
    public string Comment { get; set; }
}