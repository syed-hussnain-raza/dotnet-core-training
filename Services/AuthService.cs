using Microsoft.AspNetCore.Identity;
using MyAssignment.Constants;
using MyAssignment.Dtos;

namespace MyAssignment.Services
{
    /// <summary>
    /// Provides business logic for account registration and login, backed
    /// by ASP.NET Core Identity and JWT token generation.
    /// </summary>
    public class AuthService : IAuthService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IJwtTokenService _jwtTokenService;

        public AuthService(UserManager<IdentityUser> userManager, IJwtTokenService jwtTokenService)
        {
            _userManager = userManager;
            _jwtTokenService = jwtTokenService;
        }

        /// <summary>
        /// Creates a new Identity account. Throws an exception with the error message if it fails.
        /// </summary>
        public async Task RegisterAsync(RegisterDto dto)
        {
            IdentityResult identityResult = await CreateIdentityUserAsync(dto);
            
            if (!identityResult.Succeeded)
            {
                string errorMessage = BuildErrorMessage(identityResult);
                throw new Exception(errorMessage);
            }
        }

        /// <summary>
        /// Validates the given credentials and generates a JWT. Throws an exception if invalid.
        /// </summary>
        public async Task<string> LoginAsync(LoginDto dto)
        {
            IdentityUser? identityUser = await ValidateCredentialsAsync(dto);

            if (identityUser == null)
            {
                throw new Exception(MessagesConstants.InvalidCredentials);
            }

            string token = await GenerateTokenForUserAsync(identityUser);
            return token;
        }

        // Private Helper Methods

        /// <summary>
        /// Creates a new Identity account. Username is set equal to email,
        /// since this system does not use a separate username concept.
        /// </summary>
        private async Task<IdentityResult> CreateIdentityUserAsync(RegisterDto dto)
        {
            IdentityUser identityUser = new IdentityUser
            {
                UserName = dto.Email,
                Email = dto.Email
            };

            IdentityResult identityResult = await _userManager.CreateAsync(identityUser, dto.Password);
            return identityResult;
        }

        /// <summary>
        /// Joins all errors from a failed IdentityResult into a single
        /// space-separated message.
        /// </summary>
        private string BuildErrorMessage(IdentityResult identityResult)
        {
            string errorMessage = string.Join(" ", identityResult.Errors.Select(e => e.Description));
            return errorMessage;
        }

        /// <summary>
        /// Looks up a user by email and validates the given password.
        /// Returns the matched user if credentials are valid, otherwise null.
        /// </summary>
        private async Task<IdentityUser?> ValidateCredentialsAsync(LoginDto dto)
        {
            IdentityUser? identityUser = await _userManager.FindByEmailAsync(dto.Email);
            bool passwordValid = identityUser != null && await _userManager.CheckPasswordAsync(identityUser, dto.Password);
            IdentityUser? validatedUser = passwordValid ? identityUser : null;

            return validatedUser;
        }

        /// <summary>
        /// Fetches the user's assigned roles and generates a signed JWT
        /// embedding them as claims.
        /// </summary>
        private async Task<string> GenerateTokenForUserAsync(IdentityUser identityUser)
        {
            IList<string> roles = await _userManager.GetRolesAsync(identityUser);
            string token = _jwtTokenService.GenerateToken(identityUser, roles);

            return token;
        }
    }
}