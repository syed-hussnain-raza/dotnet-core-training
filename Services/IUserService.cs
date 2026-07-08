using MyAssignment.Models;
using MyAssignment.Dtos;

namespace MyAssignment.Services
{
    /// <summary>
    /// Defines business operations for managing users.
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Retrieves all users.
        /// </summary>
        List<User> GetAllUsers();

        /// <summary>
        /// Retrieves a single user by id, or null if none exists.
        /// </summary>
        User? GetUserById(int id);

        /// <summary>
        /// Creates a new user from the given DTO.
        /// </summary>
        User CreateUser(UserDto dto);

        /// <summary>
        /// Updates an existing user with the given DTO's values, or null if no
        /// matching user exists.
        /// </summary>
        User? UpdateUser(int id, UserDto dto);

        /// <summary>
        /// Deletes a user by id. Returns true if found and deleted.
        /// </summary>
        bool DeleteUser(int id);
    }
}