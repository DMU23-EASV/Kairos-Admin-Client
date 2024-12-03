namespace WPF_MVVM_TEMPLATE.Entitys.DTOs;

public class CreateUserDTO
{
    public string username { get; set; }
    public string password { get; set; }
    public string firstName { get; set; }
    public string lastName { get; set; }
    public int phoneNumber { get; set; }
    public string PhoneNumberLandCode { get; set; }
    public string email { get; set; }
    public int role { get; set; }
    public int status { get; set; }
    public string department { get; set; }
    public string comment { get; set; }
}