using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MyAssignment.Constants;
using MyAssignment.Data;
using MyAssignment.Dtos;
using MyAssignment.Helper;
using MyAssignment.Models;

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
        /// Retrieves a user by their unique identifier. Throws an exception if not found.
        /// </summary>
        public async Task<User> GetUserByIdAsync(string id)
        {
            User? user = await FindUserAsync(id);
            if (user == null)
            {
                throw new Exception(MessagesConstants.UserNotFound);
            }
            return user;
        }

        /// <summary>
        /// Retrieves a user by their email address. Throws an exception if not found.
        /// </summary>
        public async Task<User> GetUserByEmailAsync(string email)
        {
            User? user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
            {
                throw new Exception(MessagesConstants.UserNotFound);
            }
            return user;
        }

        /// <summary>
        /// Creates a new user in the database based on the provided DTO.
        /// </summary>
        public async Task<User> CreateUserAsync(UserDto dto)
        {
            User user = _mapper.Map<User>(dto);
            user.IsActive = true;
            user.UserName = UsernameGenerator.Generate(dto.FirstName, dto.LastName);

            _context.Users.Add(user);

            await SaveAsync();

            return user;
        }

        /// <summary>
        /// Updates an existing user in the database based on the provided DTO. Throws an exception if not found.
        /// </summary>
        public async Task<User> UpdateUserAsync(string id, UserDto dto)
        {
            User? user = await FindUserAsync(id);

            if (user == null)
            {
                throw new Exception(MessagesConstants.UserNotFound);
            }

            _mapper.Map(dto, user);
            user.UserName = UsernameGenerator.Generate(dto.FirstName, dto.LastName);
            await SaveAsync();

            return user;
        }

        /// <summary>
        /// Deletes a user from the database by their unique identifier. Throws an exception if not found.
        /// </summary>
        public async Task DeleteUserAsync(string id)
        {
            User? user = await FindUserAsync(id);

            if (user == null)
            {
                throw new Exception(MessagesConstants.UserNotFound);
            }

            _context.Users.Remove(user);
            await SaveAsync();
        }

        // Private Helper Methods

        /// <summary>
        /// Finds a user by their primary key. Uses EF Core's FindAsync().
        /// </summary>
        private async Task<User?> FindUserAsync(string id)
        {
            User? user = await _context.Users.FindAsync(id);
            return user;
        }

        /// <summary>
        /// Saves changes to the database asynchronously.
        /// </summary>
        private Task SaveAsync()
        {
            return _context.SaveChangesAsync();
        }
    }
}