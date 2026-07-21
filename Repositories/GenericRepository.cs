using Microsoft.EntityFrameworkCore;
using MyAssignment.Data;
using MyAssignment.Helper;
using System.Linq.Expressions;
using System.Linq.Dynamic.Core;
using System.Reflection;
using MyAssignment.Dtos;

namespace MyAssignment.Repositories
{
    /// <summary>
    /// EF Core-backed generic repository. Works against any entity type
    /// registered on AppDbContext via _context.Set&lt;T&gt;().
    /// </summary>
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        // Cache reflection data per entity type for maximum performance
        private static readonly Dictionary<string, PropertyInfo> _entityProperties = typeof(T)
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .ToDictionary(p => p.Name, p => p, StringComparer.OrdinalIgnoreCase);

        private static readonly List<string> _stringPropertyNames = typeof(T)
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(p => p.PropertyType == typeof(string) && p.CanRead)
            .Select(p => p.Name)
            .ToList();

        protected readonly AppDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public GenericRepository(AppDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _dbSet.AsNoTracking().ToListAsync();
        }

        public async Task<T?> GetByIdAsync(object id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.FirstOrDefaultAsync(predicate);
        }

        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public void Remove(T entity)
        {
            _dbSet.Remove(entity);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task<(List<T> Items, int TotalCount, int Page, int PageSize)> GetPagedAsync(QueryParameters queryParams)
        {
            IQueryable<T> query = _dbSet.AsNoTracking().AsQueryable();

            // Apply dynamic searching, filtering, and sorting
            query = ApplyGlobalSearch(query, queryParams.SearchTerm);
            query = ApplyColumnFilters(query, queryParams.Filters);
            query = ApplySorting(query, queryParams.SortBy, queryParams.SortDescending);

            // Execute with pagination
            int totalCount = await query.CountAsync();
            List<T> items = await query.Skip((queryParams.Page - 1) * queryParams.PageSize).Take(queryParams.PageSize).ToListAsync();

            return (items, totalCount, queryParams.Page, queryParams.PageSize);
        }

        // Private Helper Methods to simplify dynamic logic 

        private IQueryable<T> ApplyGlobalSearch(IQueryable<T> query, string? searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm) || !_stringPropertyNames.Any()) 
                return query;

            // Create a query like: Property1.Contains("term") || Property2.Contains("term")
            string searchConditions = string.Join(" || ", _stringPropertyNames.Select(name => $"{name}.Contains(@0)"));
            return query.Where(searchConditions, searchTerm);
        }

        private IQueryable<T> ApplyColumnFilters(IQueryable<T> query, Dictionary<string, string> filters)
        {
            if (filters == null || !filters.Any()) return query;

            foreach (KeyValuePair<string, string> filter in filters)
            {
                if (_entityProperties.TryGetValue(filter.Key, out PropertyInfo? prop))
                {
                    if (prop.PropertyType == typeof(string))
                    {
                        // Partial match for strings
                        query = query.Where($"{prop.Name}.Contains(@0)", filter.Value);
                    }
                    else
                    {
                        // Exact match for numbers, booleans, etc.
                        try 
                        {
                            Type targetType = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;
                            object? typedValue = Convert.ChangeType(filter.Value, targetType);
                            query = query.Where($"{prop.Name} == @0", typedValue);
                        }
                        catch { /* Ignore invalid formats */ }
                    }
                }
            }

            return query;
        }

        private IQueryable<T> ApplySorting(IQueryable<T> query, string? sortBy, bool sortDescending)
        {
            if (!string.IsNullOrWhiteSpace(sortBy) && _entityProperties.TryGetValue(sortBy, out PropertyInfo? sortProp))
            {
                string sortDirection = sortDescending ? "descending" : "ascending";
                return query.OrderBy($"{sortProp.Name} {sortDirection}");
            }
            
            // Fallback to sorting by 'Id' or the first property available
            string fallbackProp = _entityProperties.ContainsKey("Id") ? "Id" : _entityProperties.Values.First().Name;
            return query.OrderBy($"{fallbackProp} ascending");
        }
    }
}