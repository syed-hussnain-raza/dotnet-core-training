using Microsoft.EntityFrameworkCore;
using MyAssignment.Data;
using MyAssignment.Models;

namespace MyAssignment.Repositories
{
    /// <summary>
    /// EF Core-backed User repository. Inherits generic CRUD from GenericRepository<T>
    /// add User-specific queries here as needed.
    /// </summary>
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }
    }
}