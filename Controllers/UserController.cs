using Microsoft.AspNetCore.Mvc; // Provides API controller features like routing, HTTP responses
using MyAssignment.Models;      // Imports User model from Models folder

namespace MyAssignment.Controllers
{
  [ApiController]          // Enables API-specific behavior like automatic validation and binding
  [Route("api/users")]     // Base URL for all endpoints in this controller

  // Controller class handling all user-related API requests
  public class UserController: ControllerBase
  {
    // memory as a database
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
      return Ok(_users);
    }

    // Get user by id
    [HttpGet("{id}")]
    public IActionResult GetUserById(int id)
    {
      var user = FindUser(id);

      if (user == null)
      {
        return NotFound("User not found");
      }

      return Ok(user);
    }

    // Add a new user
    [HttpPost]
    public IActionResult CreateUser(User user)
    {
      // validate incoming data
      if (!IsValidUser(user))
          return BadRequest("FullName, Email and PhoneNumber are required");
      
      // auto generate id
      user.Id = GenerateId();

      // add to list
      _users.Add(user);

      // return 201 with created user data
      return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user);
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
        return NotFound("User not found");
      }

      // delete logic    
      _users.Remove(user);

      return NoContent();
    }

    // update a user
    [HttpPut("{id}")]
    public IActionResult UpdateUser(int id, User updatedUser)
    {
      var user = FindUser(id);

      if (user == null)
      {
        return NotFound("User not found for update");
      }

      // validate incoming data
      if (!IsValidUser(updatedUser))
          return BadRequest("FullName, Email and PhoneNumber are required");

      // replace properties
      user.FullName       = updatedUser.FullName;
      user.Email          = updatedUser.Email;
      user.PhoneNumber    = updatedUser.PhoneNumber;
      user.MembershipType = updatedUser.MembershipType;
      user.IsActive       = updatedUser.IsActive;

      return NoContent();
    }

    // Private Helper Methods
    
    // find user with given id
    private User? FindUser(int id)
    {
      return _users.FirstOrDefault(u => u.Id == id);
    }

    // validate user
    private bool IsValidUser(User user)
    {
      return !string.IsNullOrEmpty(user.FullName) && 
             !string.IsNullOrEmpty(user.Email) && 
             !string.IsNullOrEmpty(user.PhoneNumber);
    }

    // generate id
    private int GenerateId()
    {
        return _users.Max(u => u.Id) + 1;
    }
  }
}