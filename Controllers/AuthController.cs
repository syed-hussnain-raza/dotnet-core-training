using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyAssignment.Constants;
using MyAssignment.Dtos;
using MyAssignment.Services;
using Asp.Versioning;

namespace MyAssignment.Controllers
{
    /// <summary>
    /// Handles registration, email confirmation, and login.
    /// </summary>
    [ApiController]
    [ApiVersion(ApiVersionsConstants.V1)]
    [Route(ApiRoutesConstants.Auth)]
    [AllowAnonymous]
    public class AuthController : BaseApiController
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            IActionResult result;

            try
            {
                (bool succeeded, string errorMessage) = await _authService.RegisterAsync(dto);

                if (succeeded)
                {
                    result = Ok<object>(MessagesConstants.UserRegistered, default);
                }
                else
                {
                    result = BadRequest(errorMessage.Length > 0 ? errorMessage : MessagesConstants.RegistrationFailed);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[DEBUG] Register failed: {ex}");
                result = BadRequest(MessagesConstants.UnexpectedError);
            }

            return result;
        }

        /// <summary>
        /// Confirms the user's email and sets their first password, using
        /// the token from the confirmation link sent at registration.
        /// </summary>
        [HttpPost("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(ConfirmEmailDto dto)
        {
            IActionResult result;

            try
            {
                (bool succeeded, string errorMessage) = await _authService.ConfirmEmailAsync(dto);

                if (succeeded)
                {
                    result = Ok<object>(MessagesConstants.PasswordSetSuccess, default);
                }
                else
                {
                    result = BadRequest(errorMessage);
                }
            }
            catch (Exception)
            {
                result = BadRequest(MessagesConstants.UnexpectedError);
            }

            return result;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            IActionResult result;

            try
            {
                (bool succeeded, string token, string errorMessage) = await _authService.LoginAsync(dto);

                if (succeeded)
                {
                    result = Ok(MessagesConstants.LoginSuccess, token);
                }
                else
                {
                    result = BadRequest(errorMessage);
                }
            }
            catch (Exception)
            {
                result = BadRequest(MessagesConstants.UnexpectedError);
            }

            return result;
        }
    }
}