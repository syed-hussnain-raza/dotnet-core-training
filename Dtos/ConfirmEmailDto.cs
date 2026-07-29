using System.ComponentModel.DataAnnotations;

namespace MyAssignment.Dtos
{
    /// <summary>
    /// Submitted after the user follows the confirmation link — confirms
    /// the email and sets the account's first password in one step.
    /// </summary>
    public class ConfirmEmailDto
    {
        [Required]
        public string UserId { get; set; } = string.Empty;

        [Required]
        public string Token { get; set; } = string.Empty;

    }
}