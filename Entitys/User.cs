namespace WPF_MVVM_TEMPLATE.Entitys;

/// <summary>
/// "_id": "4824fd769171-1732698455127",
/// "Username": "Lise567",
///"Password": "hemmeligKode567",
/// "firstname": "Lise",
/// "lastname": "Andersen",
/// "phonenumber": "23456789",
/// "landcode": "+45",
/// "Email": "Lise567@gmail.com",
/// "Role": 2,
/// "Status": 1,
/// "Department": "Finance",
/// "Comment": "Søger altid forbedringer i processerne."
/// </summary>
public class User
{
    public string Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }

    public string Username { get; set; }
    public string Password { get; set; }
    public string PhoneNumber { get; set; }
    public string PhoneNumberLandCode { get; set; }
    public string Role { get; set; }
    public string Status { get; set; }
    public string Department { get; set; }
    public string Comment { get; set; }
}
