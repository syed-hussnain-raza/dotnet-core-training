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
    }
}