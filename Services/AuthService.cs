using Microsoft.AspNetCore.Identity;
using MyAssignment.Constants;
using MyAssignment.Dtos;
using AutoMapper;

namespace MyAssignment.Services
{
    /// <summary>
    /// Provides business logic for account registration and login, backed
    /// by ASP.NET Core Identity and JWT token generation.
    /// </summary>
    public class AuthService : IAuthService
    {
        private readonly UserManager<Models.User> _userManager;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly IMapper _mapper;

        public AuthService(UserManager<Models.User> userManager, IJwtTokenService jwtTokenService, IMapper mapper)
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
            Models.User user = new Models.User(dto.FirstName, dto.LastName, dto.Email, dto.PhoneNumber, dto.DateOfBirth, dto.Address);
            
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
            Models.User? user = await ValidateCredentialsAsync(dto);

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
        private async Task<Models.User?> ValidateCredentialsAsync(LoginDto dto)
        {
            Models.User? validatedUser = null;
            Models.User? user = await _userManager.FindByNameAsync(dto.UserName);
            
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
        private async Task<string> GenerateTokenForUserAsync(Models.User user)
        {
            IList<string> roles = await _userManager.GetRolesAsync(user);
            string token = _jwtTokenService.GenerateToken(user, roles);

            return token;
        }
    }
}