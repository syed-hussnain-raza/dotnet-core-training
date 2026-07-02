using Microsoft.AspNetCore.Mvc;       // Provides API controller features like routing, HTTP responses
using MyAssignment.Models;            // Imports User model from Models folder
using MyAssignment.Constants;         // Imports centralized response messages
using System.Text.RegularExpressions; // Used for email/phone format validation

namespace MyAssignment.Controllers
{
    /// <summary>
    /// Handles all user-related API requests: create, read, update, and delete.
    /// </summary>
    [ApiController]          // Enables API-specific behavior like automatic validation and binding
    [Route("api/users")]     // Base URL for all endpoints in this controller
    public class UserController : ControllerBase
    {
        // In-memory list acting as a database
        private static List<User> _users = new List<User>
        {
            new User(1, "Hussnain", "hussnain@gmail.com", "03001234567", "Premium", true),
            new User(2, "Hasnat", "hasnat@gmail.com", "03001234432", "Premium", true),
            new User(3, "Ali", "ali@gmail.com", "03007654321", "Basic", true),
            new User(4, "Sara", "sara@gmail.com", "03009876543", "Premium", false)
        };

        /// <summary>
        /// Retrieves all users.
        /// </summary>
        /// <returns>OK with the list of users.</returns>
        [HttpGet]
        public IActionResult GetAllUsers()
        {
            try
            {
                List<User> allUsers = _users;
                return Ok(allUsers);
            }
            catch (Exception)
            {
                return BadRequest(UserMessages.UnexpectedError);
            }
        }

        /// <summary>
        /// Retrieves a single user by id.
        /// </summary>
        /// <param name="id">The unique identifier of the user.</param>
        /// <returns>OK with the user if found; otherwise Bad Request.</returns>
        [HttpGet("{id}")]
        public IActionResult GetUserById(int id)
        {
            try
            {
                User? user = FindUser(id);

                if (user == null)
                {
                    return BadRequest(UserMessages.UserNotFound);
                }

                return Ok(user);
            }
            catch (Exception)
            {
                return BadRequest(UserMessages.UnexpectedError);
            }
        }

        /// <summary>
        /// Creates a new user.
        /// </summary>
        /// <param name="user">The user data to create.</param>
        /// <returns>OK with the created user if valid; otherwise Bad Request.</returns>
        [HttpPost]
        public IActionResult CreateUser(User user)
        {
            try
            {
                // Validate the incoming user data
                string? validationError = ValidateUser(user);

                if (validationError != null)
                {
                    return BadRequest(validationError);
                }

                user.Id = GenerateId();
                _users.Add(user);

                return Ok(user);
            }
            catch (Exception)
            {
                return BadRequest(UserMessages.UnexpectedError);
            }
        }

        /// <summary>
        /// Deletes an existing user by id.
        /// </summary>
        /// <param name="id">The unique identifier of the user to delete.</param>
        /// <returns>OK with a confirmation message if deleted; otherwise Bad Request.</returns>
        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id)
        {
            try
            {
                User? user = FindUser(id);

                if (user == null)
                {
                    return BadRequest(UserMessages.UserNotFound);
                }

                _users.Remove(user);

                return Ok(UserMessages.UserDeleted);
            }
            catch (Exception)
            {
                return BadRequest(UserMessages.UnexpectedError);
            }
        }

        /// <summary>
        /// Updates an existing user's details.
        /// </summary>
        /// <param name="id">The unique identifier of the user to update.</param>
        /// <param name="updatedUser">The new data to apply to the user.</param>
        /// <returns>200 OK with a confirmation message if updated; otherwise 400 Bad Request.</returns>
        [HttpPut("{id}")]
        public IActionResult UpdateUser(int id, User updatedUser)
        {
            try
            {
                User? user = FindUser(id);

                if (user == null)
                {
                    return BadRequest(UserMessages.UserNotFound);
                }

                string? validationError = ValidateUser(updatedUser);

                if (validationError != null)
                {
                    return BadRequest(validationError);
                }

                user.FullName = updatedUser.FullName;
                user.Email = updatedUser.Email;
                user.PhoneNumber = updatedUser.PhoneNumber;
                user.MembershipType = updatedUser.MembershipType;
                user.IsActive = updatedUser.IsActive;

                return Ok(UserMessages.UserUpdated);
            }
            catch (Exception)
            {
                return BadRequest(UserMessages.UnexpectedError);
            }
        }

        // Private Helper Methods

        /// <summary>
        /// Finds a user by id in the in-memory store.
        /// </summary>
        /// <param name="id">The unique identifier to search for.</param>
        /// <returns>The matching user, or null if none exists.</returns>
        private User? FindUser(int id)
        {
            User? match = _users.FirstOrDefault(u => u.Id == id);
            return match;
        }

        /// <summary>
        /// Validates a user's data.
        /// </summary>
        /// <param name="user">The user to validate.</param>
        /// <returns>An error message describing the first failure found, or null if the data is valid.</returns>
        private string? ValidateUser(User user)
        {
            // Check for null user
            if (user == null)
            {
                return UserMessages.ValidationFailed;
            }

            // Check for required fields
            if (!HasRequiredFields(user))
            {
                return UserMessages.ValidationFailed;
            }

            // Validate email format
            if (!IsValidEmail(user.Email))
            {
                return UserMessages.InvalidEmailFormat;
            }

            // Validating phone number format
            if (!IsValidPhoneNumber(user.PhoneNumber))
            {
                return UserMessages.InvalidPhoneFormat;
            }

            // MembershipType provided must be valid
            if (!IsValidMembership(user.MembershipType))
            {
                return UserMessages.InvalidMembershipType;
            }

            // If all validations pass, return null indicating no errors
            return null;
        }

        /// <summary>
        /// Checks whether all required fields have values.
        /// </summary>
        private bool HasRequiredFields(User user)
        {
            return !string.IsNullOrWhiteSpace(user.FullName) &&
                   !string.IsNullOrWhiteSpace(user.Email) &&
                   !string.IsNullOrWhiteSpace(user.PhoneNumber);
        }

        /// <summary>
        /// Validates email format.
        /// </summary>
        private bool IsValidEmail(string email)
        {
            return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        }

        /// <summary>
        /// Validates Pakistani phone number format.
        /// </summary>
        private bool IsValidPhoneNumber(string phoneNumber)
        {
            return Regex.IsMatch(phoneNumber, @"^03\d{9}$");
        }

        /// <summary>
        /// Validates membership type.
        /// </summary>
        private bool IsValidMembership(string membershipType)
        {
            return MembershipTypes.AllowedTypes.Contains(membershipType);
        }

        /// <summary>
        /// Generates the next available user id.
        /// </summary>
        /// <returns>A new unique id.</returns>
        private int GenerateId()
        {
            int nextId = _users.Count == 0 ? 1 : _users.Max(u => u.Id) + 1;
            return nextId;
        }
    }
}