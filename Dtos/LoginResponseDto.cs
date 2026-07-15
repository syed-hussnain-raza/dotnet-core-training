using MyAssignment.Constants;

namespace MyAssignment.Dtos
{
    /// <summary>
    /// Data transfer object used for returning login response.
    /// </summary>
    public class LoginResponseDto
    {
        public string Token { get; set; } = string.Empty;
        
        public UserDto User { get; set; } = null!;
    }
}
