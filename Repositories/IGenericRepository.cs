using System.Linq.Expressions;

namespace MyAssignment.Repositories
{
    /// <summary>
    /// Defines generic data-access operations reusable across any entity type.
    /// </summary>
    public interface IGenericRepository<T> where T : class
    {
        Task<List<T>> GetAllAsync();

        Task<T?> GetByIdAsync(int id);

        Task AddAsync(T entity);

        void Remove(T entity);

        Task<int> SaveChangesAsync();

        /// <summary>
        /// Retrieves a paged, optionally filtered and sorted subset of entities.
        /// </summary>
        /// <param name="page">1-based page number.</param>
        /// <param name="pageSize">Number of items per page.</param>
        /// <param name="filter">Optional predicate applied before paging (e.g. search).</param>
        /// <param name="orderBy">Optional ordering applied before paging.</param>
        Task<(List<T> Items, int TotalCount)> GetPagedAsync(
            int page,
            int pageSize,
            Expression<Func<T, bool>>? filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null);
    }
}