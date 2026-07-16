using MyAssignment.Models;

namespace MyAssignment.Repositories
{
    /// <summary>
    /// User-specific data-access operations, on top of generic CRUD.
    /// </summary>
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User?> GetByEmailAsync(string email);
    }
}