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
        public List<User> GetAllUsers()
        {
            List<User> users = _context.Users.AsNoTracking().ToList();
            return users;
        }

        /// <summary>
        /// Retrieves a user by their unique identifier.
        /// </summary>
        public User? GetUserById(int id)
        {
            User? user = FindUser(id);
            return user;
        }

        /// <summary>
        /// Creates a new user in the database based on the provided DTO.
        /// </summary>
        public User CreateUser(UserDto dto)
        {
            User user = _mapper.Map<User>(dto);
            user.IsActive = true;
            _context.Users.Add(user);
            _context.SaveChanges();
            return user;
        }

        /// <summary>
        /// Updates an existing user in the database based on the provided DTO.
        /// </summary>
        public User? UpdateUser(int id, UserDto dto)
        {
            User? user = FindUser(id);

            if (user != null)
            {
                _mapper.Map(dto, user);
                _context.SaveChanges();
            }

            return user;
        }

        /// <summary>
        /// Deletes a user from the database by their unique identifier.
        /// </summary>
        public bool DeleteUser(int id)
        {
            User? user = FindUser(id);
            bool deleted = false;

            if (user != null)
            {
                _context.Users.Remove(user);
                _context.SaveChanges();
                deleted = true;
            }

            return deleted;
        }

        // Private Helper Methods

        /// <summary>
        /// Finds a user by their primary key. Uses EF Core's Find().
        /// </summary>
        private User? FindUser(int id)
        {
            User? user = _context.Users.Find(id);
            return user;
        }
    }
}