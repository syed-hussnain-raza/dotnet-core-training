using System.ComponentModel.DataAnnotations;

namespace MyAssignment.Dtos
{
    /// <summary>
    /// No password here as the account is created passwordless,
    /// and the confirmation email lets the user set one.
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
        [RegularExpression(@"^03\d{9}$", ErrorMessage = "Invalid phone number format.")]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required]
        public DateOnly DateOfBirth { get; set; }

        public string Address { get; set; } = string.Empty;
    }
}