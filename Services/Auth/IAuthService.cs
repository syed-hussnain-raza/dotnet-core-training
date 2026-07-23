using MyAssignment.Dtos;

namespace MyAssignment.Services.Auth
{
    /// <summary>
    /// Defines business operations for account registration, email
    /// confirmation, and login.
    /// </summary>
    public interface IAuthService
    {
        Task RegisterAsync(RegisterDto dto);

        Task<string> ConfirmEmailAsync(ConfirmEmailDto dto);

        Task SetPasswordAsync(SetPasswordDto dto);

        Task<LoginResponseDto> LoginAsync(LoginDto dto);
    }
}
