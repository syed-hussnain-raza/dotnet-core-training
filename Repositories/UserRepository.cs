using Microsoft.EntityFrameworkCore;
using MyAssignment.Data;
using MyAssignment.Models;

namespace MyAssignment.Repositories
{
    /// <summary>
    /// Responsible for reading/writing User entities via AppDbContext
    /// </summary>
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves all users
        /// </summary>
        public async Task<List<User>> GetAllAsync()
        {
            List<User> users = await _context.Users.AsNoTracking().ToListAsync();
            return users;
        }

        /// <summary>
        /// Retrieves a single user by id, tracked.
        /// </summary>
        public async Task<User?> GetByIdAsync(int id)
        {
            User? user = await _context.Users.FindAsync(id);
            return user;
        }

        /// <summary>
        /// Add a new user to the context (require SaveChangesAsync call to persist changes).
        /// </summary>
        public Task AddAsync(User user)
        {
            _context.Users.Add(user);
            return Task.CompletedTask;
        }

        /// <summary>
        /// Marks a tracked user for removal (require SaveChangesAsync call to persist changes).
        /// </summary>
        public void Remove(User user)
        {
            _context.Users.Remove(user);
        }

        /// <summary>
        /// Persists all pending changes to the database.
        /// </summary>
        public async Task<int> SaveChangesAsync()
        {
            int affectedRows = await _context.SaveChangesAsync();
            return affectedRows;
        }
    }
}