using AutoMapper;
using Microsoft.AspNetCore.Mvc; // Provides API controller features like routing, HTTP responses
using MyAssignment.Models;      // Imports User model from Models folder
using MyAssignment.Helper;

namespace MyAssignment.Controllers
{
  [ApiController]          // Enables API-specific behavior like automatic validation and binding
  [Route("api/users")]     // Base URL for all endpoints in this controller

  // Controller class handling all user-related API requests
  public class UserController: ControllerBase
  {
    private readonly IMapper _mapper; // AutoMapper instance for mapping between models and DTOs memory as a database

    // Constructor to inject AutoMapper dependency
    public UserController(IMapper mapper)
    {
        _mapper = mapper;
    }

    private static List<User> _users = new List<User>
    {
        new User(1, "Hussnain", "hussnain@gmail.com", "03001234567", "Premium", true),
        new User(2, "Hasnat", "hasnat@gmail.com", "03001234432", "Premium", true),
        new User(3, "Ali", "ali@gmail.com", "03007654321", "Basic", true),
        new User(4, "Sara", "sara@gmail.com", "03009876543", "Premium", false)
    };

    // Get all users
    [HttpGet]
    public IActionResult GetAllUsers()
    {
      return Ok(ApiResponse<List<User>>.SuccessResponse("Fetching all users", _users));
    }

    // Get user by id
    [HttpGet("{id}")]
    public IActionResult GetUserById(int id)
    {
      var user = FindUser(id);

      if (user == null)
        return NotFound(ApiResponse<User>.FailResponse("User not found"));

      return Ok(ApiResponse<User>.SuccessResponse("User fetched successfully", user));
    }

    // Add a new user
    [HttpPost]
    public IActionResult CreateUser(UserDto dto)
    {
      // validate incoming data
      if (!IsValidDto(dto))
          return BadRequest(ApiResponse<User>.FailResponse("FullName, Email and PhoneNumber are required"));
      
      // Map UserDto → User
      var user = _mapper.Map<User>(dto);
      user.Id = GenerateId();
      user.IsActive = true;

      // add to list
      _users.Add(user);

      // return 201 with created user data
      return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, ApiResponse<User>.SuccessResponse("Created user successfully", user));
    }

    // delete existing user
    [HttpDelete("{id}")]
    public IActionResult DeleteUser(int id)
    { 
      // find user
      var user = FindUser(id);
      
      // if user not exits
      if (user == null)
      {
        return NotFound(ApiResponse<User>.FailResponse("User not found"));
      }

      // delete logic    
      _users.Remove(user);

      return Ok(ApiResponse<object>.SuccessResponse("Delete successfully", default));
    }

    // update a user
    [HttpPut("{id}")]
    public IActionResult UpdateUser(int id, UserDto dto)
    {
      var user = FindUser(id);

      if (user == null)
      {
        return NotFound(ApiResponse<User>.FailResponse("User not found for update"));
      }

      // validate incoming data
      if (!IsValidDto(dto))
          return BadRequest(ApiResponse<User>.FailResponse("FullName, Email and PhoneNumber are required"));

      // Map UserDto properties onto existing User object
      _mapper.Map(dto, user);

      return Ok(ApiResponse<User>.SuccessResponse("Data Updated Successfully", user));
    }

    // Private Helper Methods
    
    // find user with given id
    private User? FindUser(int id)
    {
      return _users.FirstOrDefault(u => u.Id == id);
    }

    // Validate DTO
    private bool IsValidDto(UserDto dto)
    {
        return !string.IsNullOrEmpty(dto.FullName) &&
                !string.IsNullOrEmpty(dto.Email) &&
                !string.IsNullOrEmpty(dto.PhoneNumber);
    }

    // generate id
    private int GenerateId()
    {
        return _users.Max(u => u.Id) + 1;
    }
  }
}