using MyAssignment.Models;

namespace MyAssignment.Repositories
{
    /// <summary>
    /// Defines pure data-access operations for User entities.
    /// </summary>
    public interface IUserRepository
    {
        Task<List<User>> GetAllAsync();

        Task<User?> GetByIdAsync(int id);

        Task AddAsync(User user);

        void Remove(User user);

        Task<int> SaveChangesAsync();
    }
}