using Microsoft.AspNetCore.Mvc;
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
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/users")] // dynamic version in route
    public class UserController : ControllerBase
    {
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
        public IActionResult GetAllUsers()
        {
            try
            {
                List<User> users = _userService.GetAllUsers();
                return Ok(ApiResponse<List<User>>.SuccessResponse(UserMessages.UsersFetched, users));
            }
            catch (Exception)
            {
                return BadRequest(ApiResponse<List<User>>.FailResponse(UserMessages.UnexpectedError));
            }
        }

        /// <summary>
        /// Retrieves a single user by id.
        /// </summary>
        /// <param name="id">The unique identifier of the user.</param>
        /// <returns>200 OK with the user if found; otherwise 400 Bad Request.</returns>
        [HttpGet("{id}")]
        public IActionResult GetUserById(int id)
        {
            try
            {
                User? user = _userService.GetUserById(id);

                if (user == null)
                {
                    return BadRequest(ApiResponse<User>.FailResponse(UserMessages.UserNotFound));
                }

                return Ok(ApiResponse<User>.SuccessResponse(UserMessages.UserFetched, user));
            }
            catch (Exception)
            {
                return BadRequest(ApiResponse<User>.FailResponse(UserMessages.UnexpectedError));
            }
        }

        /// <summary>
        /// Creates a new user.
        /// </summary>
        /// <param name="dto">The user data to create.</param>
        /// <returns>200 OK with the created user if valid; otherwise 400 Bad Request.</returns>
        [HttpPost]
        public IActionResult CreateUser(UserDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    string errorMessage = BuildValidationErrorMessage();
                    return BadRequest(ApiResponse<User>.FailResponse(errorMessage));
                }

                User user = _userService.CreateUser(dto);
                return Ok(ApiResponse<User>.SuccessResponse(UserMessages.UserCreated, user));
            }
            catch (Exception)
            {
                return BadRequest(ApiResponse<User>.FailResponse(UserMessages.UnexpectedError));
            }
        }

        /// <summary>
        /// Deletes an existing user by id.
        /// </summary>
        /// <param name="id">The unique identifier of the user to delete.</param>
        /// <returns>200 OK with a confirmation message if deleted; otherwise 400 Bad Request.</returns>
        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id)
        {
            try
            {
                bool deleted = _userService.DeleteUser(id);

                if (!deleted)
                {
                    return BadRequest(ApiResponse<object>.FailResponse(UserMessages.UserNotFound));
                }

                return Ok(ApiResponse<object>.SuccessResponse(UserMessages.UserDeleted, default));
            }
            catch (Exception)
            {
                return BadRequest(ApiResponse<object>.FailResponse(UserMessages.UnexpectedError));
            }
        }

        /// <summary>
        /// Updates an existing user's details.
        /// </summary>
        /// <param name="id">The unique identifier of the user to update.</param>
        /// <param name="dto">The new data to apply to the user.</param>
        /// <returns>200 OK with the updated user if valid; otherwise 400 Bad Request.</returns>
        [HttpPut("{id}")]
        public IActionResult UpdateUser(int id, UserDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    string errorMessage = BuildValidationErrorMessage();
                    return BadRequest(ApiResponse<User>.FailResponse(errorMessage));
                }

                User? user = _userService.UpdateUser(id, dto);

                if (user == null)
                {
                    return BadRequest(ApiResponse<User>.FailResponse(UserMessages.UserNotFoundForUpdate));
                }

                return Ok(ApiResponse<User>.SuccessResponse(UserMessages.UserUpdated, user));
            }
            catch (Exception)
            {
                return BadRequest(ApiResponse<User>.FailResponse(UserMessages.UnexpectedError));
            }
        }

        // Private Helper Methods

        /// <summary>
        /// Collects all current ModelState validation errors into a single message.
        /// </summary>
        /// <returns>A space-separated string of validation error messages.</returns>
        private string BuildValidationErrorMessage()
        {
            IEnumerable<string> errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage);

            return string.Join(" ", errors);
        }
    }
}