using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyAssignment.Helper;
using MyAssignment.Models;
using MyAssignment.Dtos;
using MyAssignment.Services;
using MyAssignment.Constants;
using Asp.Versioning;

namespace MyAssignment.Controllers
{
    /// <summary>
    /// Handles all user-related API requests: create, read, update, and delete.
    /// </summary>
    [ApiController]
    [ApiVersion(ApiVersionsConstants.V1)]
    [Route(ApiRoutesConstants.Users)]
    public class UserController : ControllerBase
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
                result = Ok(ApiResponse<List<User>>.SuccessResponse(MessagesConstants.UsersFetched, users));
            }
            catch (Exception)
            {
                result = BadRequest(ApiResponse<List<User>>.FailResponse(MessagesConstants.UnexpectedError));
            }

            return result;
        }

        /// <summary>
        /// Retrieves a single user by id.
        /// </summary>
        /// <param name="id">The unique identifier of the user.</param>
        /// <returns>200 OK with the user if found; otherwise 400 Bad Request.</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            IActionResult result;

            try
            {
                User? user = await _userService.GetUserByIdAsync(id);

                if (user == null)
                {
                    result = BadRequest(ApiResponse<User>.FailResponse(MessagesConstants.UserNotFound));
                }
                else
                {
                    result = Ok(ApiResponse<User>.SuccessResponse(MessagesConstants.UserFetched, user));
                }
            }
            catch (Exception)
            {
                result = BadRequest(ApiResponse<User>.FailResponse(MessagesConstants.UnexpectedError));
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
                result = Ok(ApiResponse<User>.SuccessResponse(MessagesConstants.UserCreated, user));
            }
            catch (Exception)
            {
                result = BadRequest(ApiResponse<User>.FailResponse(MessagesConstants.UnexpectedError));
            }

            return result;
        }

        /// <summary>
        /// Deletes an existing user by id.
        /// </summary>
        /// <param name="id">The unique identifier of the user to delete.</param>
        /// <returns>200 OK with a confirmation message if deleted; otherwise 400 Bad Request.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            IActionResult result;

            try
            {
                bool deleted = await _userService.DeleteUserAsync(id);

                if (!deleted)
                {
                    result = BadRequest(ApiResponse<object>.FailResponse(MessagesConstants.UserNotFound));
                }
                else
                {
                    result = Ok(ApiResponse<object>.SuccessResponse(MessagesConstants.UserDeleted, default));
                }
            }
            catch (Exception)
            {
                result = BadRequest(ApiResponse<object>.FailResponse(MessagesConstants.UnexpectedError));
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
        public async Task<IActionResult> UpdateUser(int id, UserDto dto)
        {
            IActionResult result;

            try
            {
                User? user = await _userService.UpdateUserAsync(id, dto);

                if (user == null)
                {
                    result = BadRequest(ApiResponse<User>.FailResponse(MessagesConstants.UserNotFound));
                }
                else
                {
                    result = Ok(ApiResponse<User>.SuccessResponse(MessagesConstants.UserUpdated, user));
                }
            }
            catch (Exception)
            {
                result = BadRequest(ApiResponse<User>.FailResponse(MessagesConstants.UnexpectedError));
            }

            return result;
        }
    }
}