using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyAssignment.Constants;
using MyAssignment.Dtos;
using MyAssignment.Services.Auth;
using MyAssignment.Services.Users;
using Asp.Versioning;
using MyAssignment.Shared;

namespace MyAssignment.Controllers
{
    /// <summary>
    /// Handles registration, email confirmation, and login.
    /// </summary>
    [ApiController]
    [ApiVersion(ApiVersionsConstants.V1)]
    [Route(ApiRoutesConstants.Auth)]
    public class AuthController : BaseApiController
    {
        private readonly IAuthService _authService;

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
        /// Confirms the user's email and sends a second email to set their password.
        /// </summary>
        [HttpPost(ApiRoutesConstants.ConfirmEmail)]
        public async Task<IActionResult> ConfirmEmail(ConfirmEmailDto dto)
        {
            IActionResult result;

            try
            {
                await _authService.ConfirmEmailAsync(dto);
                result = Ok<object>(MessagesConstants.EmailConfirmed, default);
            }
            catch (Exception ex)
            {
                result = BadRequest(ex.Message);
            }

            return result;
        }

        /// <summary>
        /// Sets the user's first password using the reset token.
        /// </summary>
        [HttpPost(ApiRoutesConstants.SetPassword)]
        public async Task<IActionResult> SetPassword(SetPasswordDto dto)
        {
            IActionResult result;

            try
            {
                await _authService.SetPasswordAsync(dto);
                result = Ok<object>(MessagesConstants.PasswordSetSuccess, default);
            }
            catch (Exception ex)
            {
                result = BadRequest(ex.Message);
            }

            return result;
        }

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