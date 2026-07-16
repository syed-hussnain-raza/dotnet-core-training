using Microsoft.AspNetCore.Mvc;
using MyAssignment.Models;
using MyAssignment.Dtos;
using MyAssignment.Services;
using MyAssignment.Constants;
using Asp.Versioning;
using MyAssignment.Shared;

namespace MyAssignment.Controllers
{
    /// <summary>
    /// Handles all user-related API requests: create, read, update, and delete.
    /// </summary>
    [ApiController]
    [ApiVersion(ApiVersionsConstants.V1)]
    [Route(ApiRoutesConstants.Users)]
    public class UserController : BaseApiController
    {
        /// <summary>
        /// Service responsible for user business logic and data management.
        /// </summary>
        private readonly IUserService _userService;

        /// <summary>
        /// Creates a new UserController with the given IUserService injected.
        /// </summary>
        /// <param name="userService">Service handling user business logic.</param>
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Retrieves all users.
        /// </summary>
        /// <returns>200 OK with the list of users.</returns>
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            IActionResult result;

            try
            {
                List<User> users = await _userService.GetAllUsersAsync();
                result = Ok(MessagesConstants.UsersFetched, users);
            }
            catch (Exception ex)
            {
                result = BadRequest(ex.Message);
            }

            return result;
        }

        /// <summary>
        /// Retrieves a single user by id.
        /// </summary>
        /// <param name="id">The unique identifier of the user.</param>
        /// <returns>200 OK with the user if found; otherwise 400 Bad Request.</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(string id)
        {
            IActionResult result;

            try
            {
                User user = await _userService.GetUserByIdAsync(id);
                result = Ok(MessagesConstants.UserFetched, user);
            }
            catch (Exception ex)
            {
                result = BadRequest(ex.Message);
            }

            return result;
        }

        /// <summary>
        /// Creates a new user.
        /// </summary>
        /// <param name="dto">The user data to create.</param>
        /// <returns>200 OK with the created user if valid; otherwise 400 Bad Request.</returns>
        [HttpPost]
        public async Task<IActionResult> CreateUser(UserDto dto)
        {
            IActionResult result;

            try
            {
                User user = await _userService.CreateUserAsync(dto);
                result = Ok(MessagesConstants.UserCreated, user);
            }
            catch (Exception ex)
            {
                result = BadRequest(ex.Message);
            }

            return result;
        }

        /// <summary>
        /// Deletes an existing user by id.
        /// </summary>
        /// <param name="id">The unique identifier of the user to delete.</param>
        /// <returns>200 OK with a confirmation message if deleted; otherwise 400 Bad Request.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            IActionResult result;

            try
            {
                await _userService.DeleteUserAsync(id);
                result = Ok<object>(MessagesConstants.UserDeleted, default);
            }
            catch (Exception ex)
            {
                result = BadRequest(ex.Message);
            }

            return result;
        }

        /// <summary>
        /// Updates an existing user's details.
        /// </summary>
        /// <param name="id">The unique identifier of the user to update.</param>
        /// <param name="dto">The new data to apply to the user.</param>
        /// <returns>200 OK with the updated user if valid; otherwise 400 Bad Request.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(string id, UserDto dto)
        {
            IActionResult result;

            try
            {
                User user = await _userService.UpdateUserAsync(id, dto);
                result = Ok(MessagesConstants.UserUpdated, user);
            }
            catch (Exception ex)
            {
                result = BadRequest(ex.Message);
            }

            return result;
        }
    }
}