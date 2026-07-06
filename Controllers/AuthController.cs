using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyAssignment.Constants;
using MyAssignment.Dtos;
using MyAssignment.Helper;
using MyAssignment.Services;
using Asp.Versioning;

namespace MyAssignment.Controllers
{
    /// <summary>
    /// Controller for handling user authentication, including registration and login.
    /// </summary>
    [ApiController]
    [ApiVersion(ApiVersionsConstants.V1)]
    [Route(ApiRoutesConstants.Auth)]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IJwtTokenService _jwtTokenService;

        public AuthController(UserManager<IdentityUser> userManager, IJwtTokenService jwtTokenService)
        {
            _userManager = userManager;
            _jwtTokenService = jwtTokenService;
        }

        /// <summary>
        /// Registers a new login account via Identity.
        /// </summary>
        /// <param name="dto">Email and password for the new account.</param>
        /// <returns>200 OK if registered; otherwise 400 Bad Request.</returns>
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            IActionResult result;

            try
            {
                IdentityUser identityUser = new IdentityUser
                {
                    UserName = dto.Email,
                    Email = dto.Email
                };

                IdentityResult identityResult = await _userManager.CreateAsync(identityUser, dto.Password);

                if (identityResult.Succeeded)
                {
                    result = Ok(ApiResponse<object>.SuccessResponse(MessagesConstants.UserRegistered, default));
                }
                else
                {
                    string errors = string.Join(" ", identityResult.Errors.Select(e => e.Description));
                    result = BadRequest(ApiResponse<object>.FailResponse(errors.Length > 0 ? errors : MessagesConstants.RegistrationFailed));
                }
            }
            catch (Exception)
            {
                result = BadRequest(ApiResponse<object>.FailResponse(MessagesConstants.UnexpectedError));
            }

            return result;
        }

        /// <summary>
        /// Authenticates a user and issues a JWT on success.
        /// </summary>
        /// <param name="dto">Login credentials.</param>
        /// <returns>200 OK with a JWT if valid; otherwise 400 Bad Request.</returns>
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            IActionResult result;

            try
            {
                IdentityUser? identityUser = await _userManager.FindByEmailAsync(dto.Email);
                bool passwordValid = identityUser != null && await _userManager.CheckPasswordAsync(identityUser, dto.Password);

                if (!passwordValid || identityUser == null)
                {
                    result = BadRequest(ApiResponse<object>.FailResponse(MessagesConstants.InvalidCredentials));
                }
                else
                {
                    IList<string> roles = await _userManager.GetRolesAsync(identityUser);
                    string token = _jwtTokenService.GenerateToken(identityUser, roles);
                    result = Ok(ApiResponse<string>.SuccessResponse(MessagesConstants.LoginSuccess, token));
                }
            }
            catch (Exception)
            {
                result = BadRequest(ApiResponse<object>.FailResponse(MessagesConstants.UnexpectedError));
            }

            return result;
        }
    }
}