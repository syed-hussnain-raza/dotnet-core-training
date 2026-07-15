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
        Task<List<User>> GetAllUsersAsync();

        /// <summary>
        /// Retrieves a single user by id. Throws an exception if none exists.
        /// </summary>
        Task<User> GetUserByIdAsync(string id);

        /// <summary>
        /// Retrieves a single user by email. Throws an exception if none exists.
        /// </summary>
        Task<User> GetUserByEmailAsync(string email);

        /// <summary>
        /// Creates a new user from the given DTO.
        /// </summary>
        Task<User> CreateUserAsync(UserDto dto);

        /// <summary>
        /// Updates an existing user with the given DTO's values. Throws an exception if no matching user exists.
        /// </summary>
        Task<User> UpdateUserAsync(string id, UserDto dto);

        /// <summary>
        /// Deletes a user by id. Throws an exception if no matching user exists.
        /// </summary>
        Task DeleteUserAsync(string id);
    }
}