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
            return await Microsoft.EntityFrameworkCore.EntityFrameworkQueryableExtensions.FirstOrDefaultAsync(_context.Users, u => u.Email == email);
        }
    }
}