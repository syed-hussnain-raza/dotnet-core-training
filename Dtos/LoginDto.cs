using System.ComponentModel.DataAnnotations;

namespace MyAssignment.Dtos
{
    /// <summary>
    /// Data transfer object used for user login requests.
    /// </summary>
    public class LoginDto
    {
        [Required]
        public string UserName { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;
    }
}