using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using MyAssignment.Constants;
using MyAssignment.Dtos;

namespace MyAssignment.Services
{
    /// <summary>
    /// Provides business logic for registration, email confirmation, and login.
    /// </summary>
    public class AuthService : IAuthService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly IEmailSender _emailSender;
        private readonly IConfiguration _configuration;

        public AuthService(
            UserManager<IdentityUser> userManager,
            IJwtTokenService jwtTokenService,
            IEmailSender emailSender,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _jwtTokenService = jwtTokenService;
            _emailSender = emailSender;
            _configuration = configuration;
        }

        public async Task<(bool Succeeded, string ErrorMessage)> RegisterAsync(RegisterDto dto)
        {
            IdentityResult identityResult = await CreateIdentityUserAsync(dto);
            string errorMessage = string.Empty;

            if (identityResult.Succeeded)
            {
                await SendConfirmationEmailAsync(dto.Email);
            }
            else
            {
                errorMessage = BuildErrorMessage(identityResult);
            }

            return (identityResult.Succeeded, errorMessage);
        }

        public async Task<(bool Succeeded, string ErrorMessage)> ConfirmEmailAsync(ConfirmEmailDto dto)
        {
            IdentityUser? identityUser = await _userManager.FindByEmailAsync(dto.Email);
            bool succeeded = false;
            string errorMessage = MessagesConstants.UserNotFound;

            if (identityUser != null)
            {
                bool alreadyHasPassword = await _userManager.HasPasswordAsync(identityUser);

                if (alreadyHasPassword)
                {
                    errorMessage = MessagesConstants.AlreadyHasPassword;
                }
                else
                {
                    IdentityResult confirmResult = await _userManager.ConfirmEmailAsync(identityUser, dto.Token);

                    if (!confirmResult.Succeeded)
                    {
                        errorMessage = MessagesConstants.EmailConfirmationFailed;
                    }
                    else
                    {
                        IdentityResult passwordResult = await _userManager.AddPasswordAsync(identityUser, dto.NewPassword);
                        succeeded = passwordResult.Succeeded;
                        errorMessage = succeeded ? string.Empty : BuildErrorMessage(passwordResult);
                    }
                }
            }

            return (succeeded, errorMessage);
        }

        public async Task<(bool Succeeded, string Token, string ErrorMessage)> LoginAsync(LoginDto dto)
        {
            IdentityUser? identityUser = await _userManager.FindByEmailAsync(dto.Email);
            bool succeeded = false;
            string token = string.Empty;
            string errorMessage = MessagesConstants.InvalidCredentials;

            if (identityUser != null)
            {
                if (!identityUser.EmailConfirmed)
                {
                    errorMessage = MessagesConstants.EmailNotConfirmed;
                }
                else
                {
                    bool passwordValid = await _userManager.CheckPasswordAsync(identityUser, dto.Password);

                    if (passwordValid)
                    {
                        token = await GenerateTokenForUserAsync(identityUser);
                        succeeded = true;
                        errorMessage = string.Empty;
                    }
                }
            }

            return (succeeded, token, errorMessage);
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

            // no password argument — account starts passwordless until confirmed
            IdentityResult identityResult = await _userManager.CreateAsync(identityUser);
            return identityResult;
        }

        private async Task SendConfirmationEmailAsync(string email)
        {
            IdentityUser? identityUser = await _userManager.FindByEmailAsync(email);

            if (identityUser != null)
            {
                string token = await _userManager.GenerateEmailConfirmationTokenAsync(identityUser);
                string encodedToken = Uri.EscapeDataString(token);
                string encodedEmail = Uri.EscapeDataString(email);

                string baseUrl = _configuration["Frontend:ConfirmEmailUrl"] ?? string.Empty;
                string confirmationLink = $"{baseUrl}?email={encodedEmail}&token={encodedToken}";

                string body = $"<p>Welcome! Click the link below to confirm your email and set your password:</p>" +
                              $"<p><a href=\"{confirmationLink}\">{confirmationLink}</a></p>";

                await _emailSender.SendEmailAsync(email, "Confirm your account", body);
            }
        }

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