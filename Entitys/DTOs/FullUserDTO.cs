namespace WPF_MVVM_TEMPLATE.Entitys.DTOs;
/*
 * A FullPerson Object that is API "safe" no Password is exposed.
 */
public class FullUserDTO
{
    public int Id { get; set; }
    public string username { get; set; }
    public string firstName { get; set; }
    public string lastName { get; set; }
    public int phoneNumber { get; set; }
    public string phoneNumberLandCode { get; set; }
    public string email { get; set; }
    public int role { get; set; }
    public int status { get; set; }
    public string department { get; set; }
    public string comment { get; set; }
}