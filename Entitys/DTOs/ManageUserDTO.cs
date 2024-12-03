namespace WPF_MVVM_TEMPLATE.DTO;

public class ManageUserDTO : IComparable<ManageUserDTO>
{
    public string? Username { get; set; }
    public string? Email { get; set; }
    public int? Role { get; set; }
    public int? Status { get; set; }

    public int CompareTo(ManageUserDTO? other)
    {
        if (other == null) return 1;
        return String.Compare(Username, other.Username, StringComparison.Ordinal);
    }

    public override string ToString()
    {
        return "Username: " + Username + ", Email: " + Email + ", Role: " + Role + ", Status: " + Status;
    }
}