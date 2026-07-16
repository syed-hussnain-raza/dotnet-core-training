using MyAssignment.Dtos;

namespace MyAssignment.Services
{
    /// <summary>
    /// Defines business operations for account registration and login.
    /// </summary>
    public interface IAuthService
    {
        /// <summary>
        /// Registers a new login account. Throws an exception if it fails.
        /// </summary>
        Task RegisterAsync(RegisterDto dto);

        /// <summary>
        /// Validates credentials and issues a JWT on success. Throws an exception if it fails.
        /// </summary>
        Task<LoginResponseDto> LoginAsync(LoginDto dto);
    }
}