namespace MyAssignment.Models
{
    public class UserDto
    {
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string MembershipType { get; set; } = "Basic";

        // Method to validate the DTO
        public bool IsValid()
        {
            return !string.IsNullOrEmpty(FullName) &&
                   !string.IsNullOrEmpty(Email) &&
                   !string.IsNullOrEmpty(PhoneNumber);
        }
    }
}