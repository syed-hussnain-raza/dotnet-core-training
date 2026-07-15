using System.ComponentModel.DataAnnotations;

namespace MyAssignment.Dtos
{
    /// <summary>
    /// Data transfer object used for registering a new login account via Identity.
    /// </summary>
    public class RegisterDto
    {
        [Required]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;

        [Required]
        [RegularExpression(@"^03\d{9}$", ErrorMessage = "Invalid phone number format.")]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required]
        public DateOnly DateOfBirth { get; set; }

        public string Address { get; set; } = string.Empty;
    }
}