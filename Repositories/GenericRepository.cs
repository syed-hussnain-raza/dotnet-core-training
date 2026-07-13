using Microsoft.EntityFrameworkCore;
using MyAssignment.Data;
using MyAssignment.Helper;
using System.Linq.Expressions;

namespace MyAssignment.Repositories
{
    /// <summary>
    /// EF Core-backed generic repository. Works against any entity type
    /// registered on AppDbContext via _context.Set&lt;T&gt;().
    /// </summary>
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly AppDbContext _context;
        private readonly DbSet<T> _dbSet;

        public GenericRepository(AppDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task<List<T>> GetAllAsync()
        {
            List<T> entities = await _dbSet.AsNoTracking().ToListAsync();
            return entities;
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            T? entity = await _dbSet.FindAsync(id);
            return entity;
        }

        public Task AddAsync(T entity)
        {
            _dbSet.Add(entity);
            return Task.CompletedTask;
        }

        public void Remove(T entity)
        {
            _dbSet.Remove(entity);
        }

        public async Task<int> SaveChangesAsync()
        {
            int affectedRows = await _context.SaveChangesAsync();
            return affectedRows;
        }

        public Task<(List<T> Items, int TotalCount)> GetPagedAsync(
            int page,
            int pageSize,
            Expression<Func<T, bool>>? filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null)
        {
            return _dbSet.AsNoTracking().ToPagedResultAsync(page, pageSize, filter, orderBy);
        }
    }
}