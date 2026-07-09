using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MyAssignment.Data;
using MyAssignment.Models;
using MyAssignment.Dtos;

namespace MyAssignment.Services
{
    /// <summary>
    /// Provides business logic for managing users, backed by EF Core - SQL Server.
    /// </summary>
    public class UserService : IUserService
    {
        /// <summary>
        /// The database context for accessing user data.
        /// </summary>
        private readonly AppDbContext _context;

        /// <summary>
        /// The AutoMapper instance for mapping between DTOs and models.
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserService"/> class.
        /// </summary>
        public UserService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        /// <summary>
        /// Retrieves all users from the database.
        /// </summary>
        public async Task<List<User>> GetAllUsersAsync()
        {
            List<User> users = await _context.Users.AsNoTracking().ToListAsync();
            return users;
        }

        /// <summary>
        /// Retrieves a user by their unique identifier.
        /// </summary>
        public async Task<User?> GetUserByIdAsync(int id)
        {
            User? user = await FindUserAsync(id);
            return user;
        }

        /// <summary>
        /// Creates a new user in the database based on the provided DTO.
        /// </summary>
        public async Task<User> CreateUserAsync(UserDto dto)
        {
            User user = _mapper.Map<User>(dto);
            user.IsActive = true;
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        /// <summary>
        /// Updates an existing user in the database based on the provided DTO.
        /// </summary>
        public async Task<User?> UpdateUserAsync(int id, UserDto dto)
        {
            User? user = await FindUserAsync(id);

            if (user != null)
            {
                _mapper.Map(dto, user);
                await _context.SaveChangesAsync();
            }

            return user;
        }

        /// <summary>
        /// Deletes a user from the database by their unique identifier.
        /// </summary>
        public async Task<bool> DeleteUserAsync(int id)
        {
            User? user = await FindUserAsync(id);
            bool deleted = false;

            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
                deleted = true;
            }

            return deleted;
        }

        // Private Helper Methods

        /// <summary>
        /// Finds a user by their primary key. Uses EF Core's FindAsync().
        /// </summary>
        private async Task<User?> FindUserAsync(int id)
        {
            User? user = await _context.Users.FindAsync(id);
            return user;
        }
    }
}