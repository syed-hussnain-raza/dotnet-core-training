using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using MyAssignment.Options;
using MyAssignment.Constants;
using MyAssignment.Dtos;
using AutoMapper;
using MyAssignment.Helper;
using MyAssignment.Models;
using MyAssignment.Services.Email;

namespace MyAssignment.Services.Auth
{
    /// <summary>
    /// Provides business logic for registration, email confirmation, and login.
    /// </summary>
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly IEmailService _emailService;
        private readonly IMapper _mapper;

        public AuthService(
            UserManager<User> userManager,
            IJwtTokenService jwtTokenService,
            IEmailService emailService,
            IMapper mapper)
        {
            _userManager = userManager;
            _jwtTokenService = jwtTokenService;
            _emailService = emailService;
            _mapper = mapper;
        }

        public async Task RegisterAsync(RegisterDto dto)
        {
            IdentityResult identityResult = await CreateIdentityUserAsync(dto);

            if (identityResult.Succeeded)
            {
                User? user = await _userManager.FindByEmailAsync(dto.Email);
                if (user != null)
                {
                    string token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    await _emailService.SendConfirmationEmailAsync(user, token);
                }
            }
            else
            {
                throw new Exception(MessagesConstants.RegistrationFailed);
            }
        }

        public async Task ConfirmEmailAsync(ConfirmEmailDto dto)
        {
            User? user = await _userManager.FindByIdAsync(dto.UserId);

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

            // Generate password reset token
            string resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);

            // Send an email with the link to set the password
            await _emailService.SendPasswordSetEmailAsync(user, resetToken);
        }

        public async Task SetPasswordAsync(SetPasswordDto dto)
        {
            User? user = await _userManager.FindByIdAsync(dto.UserId);

            if (user == null)
            {
                throw new Exception(MessagesConstants.UserNotFound);
            }

            bool alreadyHasPassword = await _userManager.HasPasswordAsync(user);

            if (alreadyHasPassword)
            {
                throw new Exception(MessagesConstants.AlreadyHasPassword);
            }

            // Since user has no password yet, ResetPasswordAsync is the secure way to use the reset token to set the first password
            IdentityResult passwordResult = await _userManager.ResetPasswordAsync(user, dto.Token, dto.NewPassword);

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
