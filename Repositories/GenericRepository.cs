using Microsoft.EntityFrameworkCore;
using MyAssignment.Data;
using MyAssignment.Helper;
using System.Linq.Expressions;
using System.Linq.Dynamic.Core;
using System.Reflection;
using MyAssignment.Constants;

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

        public async Task<T?> GetByIdAsync(object id)
        {
            T? entity = await _dbSet.FindAsync(id);
            return entity;
        }

        public async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.FirstOrDefaultAsync(predicate);
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

        public Task<int> SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }

        public Task<(List<T> Items, int TotalCount)> GetPagedAsync(
            int page,
            int pageSize,
            Expression<Func<T, bool>>? filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null)
        {
            return _dbSet.AsNoTracking().ToPagedResultAsync(page, pageSize, filter, orderBy);
        }

        public async Task<(List<T> Items, int TotalCount)> GetPagedDynamicAsync(Dictionary<string, string> queryParams)
        {
            IQueryable<T> query = _dbSet.AsNoTracking().AsQueryable();

            // Extract basic parameters
            ExtractParameters(queryParams, out int page, out int pageSize, out string? searchTerm, out string? sortBy, out bool sortDesc, out Dictionary<string, string> filters);

            // Apply dynamic searching, filtering, and sorting
            query = ApplyGlobalSearch(query, searchTerm);
            query = ApplyColumnFilters(query, filters);
            query = ApplySorting(query, sortBy, sortDesc);

            // Execute with pagination
            int totalCount = await query.CountAsync();
            List<T> items = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            return (items, totalCount);
        }

        // --- Private Helper Methods to simplify dynamic logic ---

        private void ExtractParameters(Dictionary<string, string> queryParams, out int page, out int pageSize, out string? searchTerm, out string? sortBy, out bool sortDescending, out Dictionary<string, string> filters)
        {
            page = 1;
            pageSize = 10;
            searchTerm = null;
            sortBy = null;
            sortDescending = false;
            filters = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            foreach (KeyValuePair<string, string> kvp in queryParams)
            {
                string key = kvp.Key.ToLower();
                string value = kvp.Value;

                if (key == QueryConstants.Page && int.TryParse(value, out int p)) page = p;
                else if (key == QueryConstants.PageSize && int.TryParse(value, out int ps)) pageSize = ps;
                else if (key == QueryConstants.SearchTerm) searchTerm = value;
                else if (key == QueryConstants.SortBy) sortBy = value;
                else if (key == QueryConstants.SortDescending && bool.TryParse(value, out bool sd)) sortDescending = sd;
                else filters.Add(kvp.Key, value);
            }
        }

        private IQueryable<T> ApplyGlobalSearch(IQueryable<T> query, string? searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm)) return query;

            // Find all string properties on the entity
            List<string> stringProperties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.PropertyType == typeof(string) && p.CanRead)
                .Select(p => p.Name)
                .ToList();

            if (stringProperties.Any())
            {
                // Create a query like: Property1.Contains("term") || Property2.Contains("term")
                string searchConditions = string.Join(" || ", stringProperties.Select(p => $"{p}.Contains(@0)"));
                return query.Where(searchConditions, searchTerm);
            }

            return query;
        }

        private IQueryable<T> ApplyColumnFilters(IQueryable<T> query, Dictionary<string, string> filters)
        {
            if (!filters.Any()) return query;

            Dictionary<string, PropertyInfo> entityProperties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .ToDictionary(p => p.Name, p => p, StringComparer.OrdinalIgnoreCase);

            foreach (KeyValuePair<string, string> filter in filters)
            {
                if (entityProperties.TryGetValue(filter.Key, out PropertyInfo? prop))
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
            Dictionary<string, PropertyInfo> entityProperties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .ToDictionary(p => p.Name, p => p, StringComparer.OrdinalIgnoreCase);

            if (!string.IsNullOrWhiteSpace(sortBy) && entityProperties.TryGetValue(sortBy, out PropertyInfo? sortProp))
            {
                string sortDirection = sortDescending ? QueryConstants.Descending : QueryConstants.Ascending;
                return query.OrderBy($"{sortProp.Name} {sortDirection}");
            }
            
            // Fallback to sorting by 'Id' or the first property available
            string fallbackProp = entityProperties.ContainsKey(QueryConstants.Id) ? QueryConstants.Id : entityProperties.Values.First().Name;
            return query.OrderBy($"{fallbackProp} {QueryConstants.Ascending}");
        }
    }
}