using Microsoft.AspNetCore.Identity;
using MyAssignment.Constants;
using MyAssignment.Dtos;
using AutoMapper;
using MyAssignment.Helper;
using MyAssignment.Models;

namespace MyAssignment.Services
{
    /// <summary>
    /// Provides business logic for account registration and login, backed
    /// by ASP.NET Core Identity and JWT token generation.
    /// </summary>
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly IMapper _mapper;

        public AuthService(UserManager<User> userManager, IJwtTokenService jwtTokenService, IMapper mapper)
        {
            _userManager = userManager;
            _jwtTokenService = jwtTokenService;
            _mapper = mapper;
        }

        /// <summary>
        /// Creates a new Identity account. Throws an exception with the error message if it fails.
        /// </summary>
        public async Task RegisterAsync(RegisterDto dto)
        {
            User user = _mapper.Map<User>(dto);
            user.UserName = UsernameGenerator.Generate(dto.FirstName, dto.LastName);
            
            IdentityResult identityResult = await _userManager.CreateAsync(user, dto.Password);
            
            if (!identityResult.Succeeded)
            {
                throw new Exception(MessagesConstants.RegistrationFailed);
            }
        }

        /// <summary>
        /// Validates the given credentials and generates a JWT. Throws an exception if invalid.
        /// </summary>
        public async Task<LoginResponseDto> LoginAsync(LoginDto dto)
        {
            User? user = await ValidateCredentialsAsync(dto);

            if (user == null)
            {
                throw new Exception(MessagesConstants.InvalidCredentials);
            }

            string token = await GenerateTokenForUserAsync(user);
            
            UserDto userDto = _mapper.Map<UserDto>(user);

            return new LoginResponseDto
            {
                Token = token,
                User = userDto
            };
        }

        // Private Helper Methods



        /// <summary>
        /// Looks up a user by username and validates the given password.
        /// Returns the matched user if credentials are valid, otherwise null.
        /// </summary>
        private async Task<User?> ValidateCredentialsAsync(LoginDto dto)
        {
            User? validatedUser = null;
            User? user = await _userManager.FindByNameAsync(dto.UserName);
            
            if (user != null)
            {
                bool isPasswordValid = await _userManager.CheckPasswordAsync(user, dto.Password);
                
                if (isPasswordValid)
                {
                    validatedUser = user;
                }
            }

            return validatedUser;
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