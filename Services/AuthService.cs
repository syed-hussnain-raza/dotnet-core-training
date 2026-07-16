using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using MyAssignment.Options;
using MyAssignment.Constants;
using MyAssignment.Dtos;
using AutoMapper;
using MyAssignment.Helper;
using MyAssignment.Models;

namespace MyAssignment.Services
{
    /// <summary>
    /// Provides business logic for registration, email confirmation, and login.
    /// </summary>
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly IEmailSender _emailSender;
        private readonly IOptions<FrontendSettings> _frontendSettings;
        private readonly IMapper _mapper;

        public AuthService(
            UserManager<User> userManager,
            IJwtTokenService jwtTokenService,
            IEmailSender emailSender,
            IOptions<FrontendSettings> frontendSettings,
            IMapper mapper)
        {
            _userManager = userManager;
            _jwtTokenService = jwtTokenService;
            _emailSender = emailSender;
            _frontendSettings = frontendSettings;
            _mapper = mapper;
        }

        public async Task RegisterAsync(RegisterDto dto)
        {
            IdentityResult identityResult = await CreateIdentityUserAsync(dto);

            if (identityResult.Succeeded)
            {
                await SendConfirmationEmailAsync(dto.Email);
            }
            else
            {
                throw new Exception(MessagesConstants.RegistrationFailed);
            }
        }

        public async Task ConfirmEmailAsync(ConfirmEmailDto dto)
        {
            User? user = await _userManager.FindByEmailAsync(dto.Email);

            if (user == null)
            {
                throw new Exception(MessagesConstants.UserNotFound);
            }

            bool alreadyHasPassword = await _userManager.HasPasswordAsync(user);

            if (alreadyHasPassword)
            {
                throw new Exception(MessagesConstants.AlreadyHasPassword);
            }

            IdentityResult confirmResult = await _userManager.ConfirmEmailAsync(user, dto.Token);

            if (!confirmResult.Succeeded)
            {
                throw new Exception(MessagesConstants.EmailConfirmationFailed);
            }

            IdentityResult passwordResult = await _userManager.AddPasswordAsync(user, dto.NewPassword);

            if (!passwordResult.Succeeded)
            {
                throw new Exception(MessagesConstants.UnexpectedError);
            }
        }

        public async Task<LoginResponseDto> LoginAsync(LoginDto dto)
        {
            User? user = await _userManager.FindByNameAsync(dto.UserName);

            if (user == null)
            {
                throw new Exception(MessagesConstants.InvalidCredentials);
            }

            if (!user.EmailConfirmed)
            {
                throw new Exception(MessagesConstants.InvalidCredentials);
            }

            bool passwordValid = await _userManager.CheckPasswordAsync(user, dto.Password);

            if (!passwordValid)
            {
                throw new Exception(MessagesConstants.InvalidCredentials);
            }

            string token = await GenerateTokenForUserAsync(user);
            UserResponseDto userDto = _mapper.Map<UserResponseDto>(user);

            return new LoginResponseDto
            {
                User = userDto,
                Token = token
            };
        }

        // Private Helper Methods

        private async Task<IdentityResult> CreateIdentityUserAsync(RegisterDto dto)
        {
            User user = _mapper.Map<User>(dto);
            user.UserName = UsernameGenerator.Generate(dto.FirstName, dto.LastName);

            // no password argument — account starts passwordless until confirmed
            IdentityResult identityResult = await _userManager.CreateAsync(user);
            return identityResult;
        }

        private async Task SendConfirmationEmailAsync(string email)
        {
            User? user = await _userManager.FindByEmailAsync(email);

            if (user != null)
            {
                string token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                string confirmationLink = $"{_frontendSettings.Value.ConfirmEmailUrl}?email={Uri.EscapeDataString(email)}&token={Uri.EscapeDataString(token)}";

                string body = $"<p>Welcome! Click the link below to confirm your email and set your password:</p>" +
                              $"<p><a href=\"{confirmationLink}\">{confirmationLink}</a></p>";

                await _emailSender.SendEmailAsync(email, "Confirm your account", body);
            }
        }


        /// <summary>
        /// Fetches the user's assigned roles and generates a signed JWT
        /// embedding them as claims.
        /// </summary>
        private async Task<string> GenerateTokenForUserAsync(User user)
        {
            IList<string> roles = await _userManager.GetRolesAsync(user);
            return _jwtTokenService.GenerateToken(user, roles);
        }
    }
}