namespace MyAssignment.Models
{
  // Represents a single User in the system
  public class User
  {
    // Properties (like private variable, with getter and setter)
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string MembershipType { get; set; } = "Basic";
    public bool IsActive { get; set; } = true;

    // Empty Constructor
    public User () { }

    // Constructor with all the fields
    public User (int id, string fullName, string email, string phoneNumber, string membershipType = "Basic", bool isActive = true)
    {
      Id  = id;
      FullName = fullName;
      Email = email;
      PhoneNumber = phoneNumber;
      MembershipType = membershipType;
      IsActive = isActive;
    }
  }
}