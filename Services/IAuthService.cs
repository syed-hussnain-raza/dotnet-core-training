using MyAssignment.Dtos;

namespace MyAssignment.Services
{
    /// <summary>
    /// Defines business operations for account registration, email
    /// confirmation, and login.
    /// </summary>
    public interface IAuthService
    {
        Task RegisterAsync(RegisterDto dto);

        Task ConfirmEmailAsync(ConfirmEmailDto dto);

        Task<LoginResponseDto> LoginAsync(LoginDto dto);
    }
}