using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyAssignment.Constants;
using MyAssignment.Dtos;
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
    public class AuthController : BaseApiController
    {
        private readonly IAuthService _authService;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthController"/> class.
        /// </summary>
        /// <param name="authService"></param>
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// Registers a new login account via Identity.
        /// </summary>
        /// <param name="dto">Email and password for the new account.</param>
        /// <returns>200 OK if registered; otherwise 400 Bad Request.</returns>
        [HttpPost(ApiRoutesConstants.Register)]
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
            catch (Exception)
            {
                result = BadRequest(MessagesConstants.UnexpectedError);
            }

            return result;
        }

        /// <summary>
        /// Authenticates a user and issues a JWT on success.
        /// </summary>
        /// <param name="dto">Login credentials.</param>
        /// <returns>200 OK with a JWT if valid; otherwise 400 Bad Request.</returns>
        [HttpPost(ApiRoutesConstants.Login)]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            IActionResult result;

            try
            {
                (bool succeeded, string token) = await _authService.LoginAsync(dto);

                if (succeeded)
                {
                    result = Ok(MessagesConstants.LoginSuccess, token);
                }
                else
                {
                    result = BadRequest(MessagesConstants.InvalidCredentials);
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