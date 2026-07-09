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
        /// Retrieves a single user by id, or null if none exists.
        /// </summary>
        Task<User?> GetUserByIdAsync(int id);

        /// <summary>
        /// Creates a new user from the given DTO.
        /// </summary>
        Task<User> CreateUserAsync(UserDto dto);

        /// <summary>
        /// Updates an existing user with the given DTO's values, or null if no
        /// matching user exists.
        /// </summary>
        Task<User?> UpdateUserAsync(int id, UserDto dto);

        /// <summary>
        /// Deletes a user by id. Returns true if found and deleted.
        /// </summary>
        Task<bool> DeleteUserAsync(int id);
    }
}