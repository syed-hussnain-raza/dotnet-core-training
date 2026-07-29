using System.ComponentModel.DataAnnotations;

namespace MyAssignment.Dtos
{
    /// <summary>
    /// Submitted to set the password after confirming the email.
    /// </summary>
    public class SetPasswordDto
    {
        [Required]
        public string UserId { get; set; } = string.Empty;

        [Required]
        public string Token { get; set; } = string.Empty;

        [Required]
        [MinLength(6)]
        public string NewPassword { get; set; } = string.Empty;
    }
}
