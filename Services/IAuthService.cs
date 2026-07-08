using MyAssignment.Dtos;

namespace MyAssignment.Services
{
    /// <summary>
    /// Defines business operations for account registration and login.
    /// </summary>
    public interface IAuthService
    {
        /// <summary>
        /// Registers a new login account. Returns whether it succeeded and,
        /// if not, a combined error message describing why.
        /// </summary>
        Task<(bool Succeeded, string ErrorMessage)> RegisterAsync(RegisterDto dto);

        /// <summary>
        /// Validates credentials and issues a JWT on success. Returns whether
        /// it succeeded and, if so, the generated token.
        /// </summary>
        Task<(bool Succeeded, string Token)> LoginAsync(LoginDto dto);
    }
}