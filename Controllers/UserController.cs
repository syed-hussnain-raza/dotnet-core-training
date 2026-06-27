using Microsoft.AspNetCore.Mvc;
using MyAssignment.Helper;
using MyAssignment.Models;
using MyAssignment.Services;

namespace MyAssignment.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        // Inject IUserService via constructor - no more IMapper needed here
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        // GET api/users
        [HttpGet]
        public IActionResult GetAllUsers()
        {
            var users = _userService.GetAllUsers();
            return Ok(ApiResponse<List<User>>.SuccessResponse("Fetching all users", users));
        }

        // GET api/users/{id}
        [HttpGet("{id}")]
        public IActionResult GetUserById(int id)
        {
            var user = _userService.GetUserById(id);

            if (user == null)
                return NotFound(ApiResponse<User>.FailResponse("User not found"));

            return Ok(ApiResponse<User>.SuccessResponse("User fetched successfully", user));
        }

        // POST api/users
        [HttpPost]
        public IActionResult CreateUser(UserDto dto)
        {
            if (string.IsNullOrEmpty(dto.FullName) ||
                string.IsNullOrEmpty(dto.Email) ||
                string.IsNullOrEmpty(dto.PhoneNumber))
                return BadRequest(ApiResponse<User>.FailResponse("FullName, Email and PhoneNumber are required"));

            var user = _userService.CreateUser(dto);
            return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, ApiResponse<User>.SuccessResponse("Created user successfully", user));
        }

        // DELETE api/users/{id}
        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id)
        {
            var deleted = _userService.DeleteUser(id);

            if (!deleted)
                return NotFound(ApiResponse<User>.FailResponse("User not found"));

            return Ok(ApiResponse<object>.SuccessResponse("Deleted successfully", default));
        }

        // PUT api/users/{id}
        [HttpPut("{id}")]
        public IActionResult UpdateUser(int id, UserDto dto)
        {
            if (string.IsNullOrEmpty(dto.FullName) ||
                string.IsNullOrEmpty(dto.Email) ||
                string.IsNullOrEmpty(dto.PhoneNumber))
                return BadRequest(ApiResponse<User>.FailResponse("FullName, Email and PhoneNumber are required"));

            var user = _userService.UpdateUser(id, dto);

            if (user == null)
                return NotFound(ApiResponse<User>.FailResponse("User not found for update"));

            return Ok(ApiResponse<User>.SuccessResponse("Data Updated Successfully", user));
        }
    }
}