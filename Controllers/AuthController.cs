using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyAssignment.Constants;
using MyAssignment.Dtos;
using MyAssignment.Services;
using Asp.Versioning;
using MyAssignment.Shared;

namespace MyAssignment.Controllers
{
    /// <summary>
        /// Controller for handling user authentication, including registration and login.
    /// </summary>
    [ApiController]
    [ApiVersion(ApiVersionsConstants.V1)]
    [Route(ApiRoutesConstants.Auth)]
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

        // There should be one admin that can add users, this admin is added manually in the database, and the admin can add users via the Register endpoint
        [HttpPost(ApiRoutesConstants.Register)]
        [Authorize(Roles = RolesConstants.Admin)]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            IActionResult result;

            try
            {
                await _authService.RegisterAsync(dto);
                result = Ok<object>(MessagesConstants.UserRegistered, default);
            }
            catch (Exception ex)
            {
                result = BadRequest(ex.Message);
            }

            return result;
        }

        /// <summary>
        /// Authenticates a user and issues a JWT on success.
        /// </summary>
        /// <param name="dto">Login credentials.</param>
        /// <returns>200 OK with a JWT if valid; otherwise 400 Bad Request.</returns>
        [HttpPost(ApiRoutesConstants.Login)]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            IActionResult result;

            try
            {
                LoginResponseDto response = await _authService.LoginAsync(dto);
                result = Ok(MessagesConstants.LoginSuccess, response);
            }
            catch (Exception ex)
            {
                result = BadRequest(ex.Message);
            }

            return result;
        }
    }
}