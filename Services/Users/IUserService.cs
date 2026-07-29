using MyAssignment.Models;
using MyAssignment.Dtos;
using MyAssignment.Helper;

namespace MyAssignment.Services.Users
{
    /// <summary>
    /// Defines business operations for managing users.
    /// </summary>
    public interface IUserService
    {


        /// <summary>
        /// Retrieves a single user by id. Throws an exception if none exists.
        /// </summary>
        Task<UserResponseDto> GetUserByIdAsync(string id);

        /// <summary>
        /// Retrieves a single user by email. Throws an exception if none exists.
        /// </summary>
        Task<UserResponseDto> GetUserByEmailAsync(string email);

        /// <summary>
        /// Creates a new user from the given DTO.
        /// </summary>
        Task<UserResponseDto> CreateUserAsync(UserDto dto);

        /// <summary>
        /// Updates an existing user with the given DTO's values. Throws an exception if no matching user exists.
        /// </summary>
        Task<UserResponseDto> UpdateUserAsync(string id, UserDto dto);

        /// <summary>
        /// Deletes a user by id. Throws an exception if no matching user exists.
        /// </summary>
        Task<bool> DeleteUserAsync(string id);

        /// <summary>
        /// Retrieves a paged, searchable, sortable list of users.
        /// </summary>
        Task<PagedResult<UserResponseDto>> GetUsersPagedAsync(QueryParameters queryParams);
    }
}
