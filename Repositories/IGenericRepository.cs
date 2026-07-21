using System.Linq.Expressions;
using MyAssignment.Dtos;

namespace MyAssignment.Repositories
{
    /// <summary>
    /// Defines generic data-access operations reusable across any entity type.
    /// </summary>
    public interface IGenericRepository<T> where T : class
    {
        Task<List<T>> GetAllAsync();

        Task<T?> GetByIdAsync(object id);

        Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate);

        Task AddAsync(T entity);

        void Remove(T entity);

        Task<int> SaveChangesAsync();

        Task<(List<T> Items, int TotalCount, int Page, int PageSize)> GetPagedAsync(QueryParameters queryParams);
    }
}