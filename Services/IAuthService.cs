using MyAssignment.Dtos;

namespace MyAssignment.Services
{
    /// <summary>
    /// Defines business operations for account registration, email
    /// confirmation, and login.
    /// </summary>
    public interface IAuthService
    {
        Task<(bool Succeeded, string ErrorMessage)> RegisterAsync(RegisterDto dto);

        Task<(bool Succeeded, string ErrorMessage)> ConfirmEmailAsync(ConfirmEmailDto dto);

        Task<(bool Succeeded, string Token, string ErrorMessage)> LoginAsync(LoginDto dto);
    }
}