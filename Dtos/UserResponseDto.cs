namespace MyAssignment.Dtos
{
    /// <summary>
    /// Data transfer object used for returning a user safely without exposing internal Identity fields.
    /// </summary>
    public class UserResponseDto
    {
        public string Id { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateOnly DateOfBirth { get; set; }
        public string Address { get; set; } = string.Empty;
        public string MembershipType { get; set; } = string.Empty;
    }
}
