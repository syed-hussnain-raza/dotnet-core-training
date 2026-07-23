using Microsoft.EntityFrameworkCore;
using MyAssignment.Data;
using MyAssignment.Helper;
using System.Linq.Expressions;
using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore.Metadata;
using MyAssignment.Dtos;

namespace MyAssignment.Repositories
{
    /// <summary>
    /// EF Core-backed generic repository. Works against any entity type
    /// registered on AppDbContext via _context.Set&lt;T&gt;().
    /// </summary>
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly Dictionary<string, IProperty> _entityProperties;
        private readonly List<string> _stringPropertyNames;

        protected readonly AppDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public GenericRepository(AppDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();

            var entityType = _context.Model.FindEntityType(typeof(T))!;
            var properties = entityType.GetProperties();
            
            _entityProperties = properties.ToDictionary(p => p.Name, p => p, StringComparer.OrdinalIgnoreCase);
            
            _stringPropertyNames = properties
                .Where(p => p.ClrType == typeof(string))
                .Select(p => p.Name)
                .ToList();
        }

        /// <summary>
        /// Retrieves all entities of type T from the database without tracking.
        /// </summary>
        /// <returns></returns>
        public async Task<List<T>> GetAllAsync()
        {
            return await _dbSet.AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// Retrieves an entity of type T by its primary key. Returns null if not found.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<T?> GetByIdAsync(object id)
        {
            return await _dbSet.FindAsync(id);
        }

        /// <summary>
        /// Retrieves the first entity of type T that matches the given predicate. Returns null if none found.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.FirstOrDefaultAsync(predicate);
        }

        /// <summary>
        /// Adds a new entity of type T to the database context.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        /// <summary>
        /// Removes an existing entity of type T from the database context.
        /// </summary>
        /// <param name="entity"></param>
        public void Remove(T entity)
        {
            _dbSet.Remove(entity);
        }

        /// <summary>
        /// Saves all changes made in the context to the database asynchronously.
        /// </summary>
        /// <returns></returns>
        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Retrieves a paginated list of entities of type T based on the provided query parameters,
        /// including dynamic searching, filtering, and sorting.
        /// </summary>
        /// <param name="queryParams"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Applies a global search across all string properties of the entity type T using the provided search term.
        /// </summary>
        /// <param name="query"></param>
        /// <param name="searchTerm"></param>
        /// <returns></returns>
        private IQueryable<T> ApplyGlobalSearch(IQueryable<T> query, string? searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm) || !_stringPropertyNames.Any()) 
                return query;

            // Create a query like: Property1.Contains("term") || Property2.Contains("term")
            string searchConditions = string.Join(" || ", _stringPropertyNames.Select(name => $"{name}.Contains(@0)"));
            return query.Where(searchConditions, searchTerm);
        }

        /// <summary>
        /// Applies column-specific filters to the query based on the provided dictionary of filters.
        /// </summary>
        /// <param name="query"></param>
        /// <param name="filters"></param>
        /// <returns></returns>
        private IQueryable<T> ApplyColumnFilters(IQueryable<T> query, Dictionary<string, string> filters)
        {
            if (filters == null || !filters.Any()) return query;

            foreach (var filter in filters)
            {
                // If column doesn't exist, skip it.
                if (!_entityProperties.TryGetValue(filter.Key, out IProperty? prop))
                    continue;

                try 
                {
                    // Text Search (Partial Match)
                    if (prop.ClrType == typeof(string))
                    {
                        query = query.Where($"{prop.Name}.Contains(@0)", filter.Value);
                    }
                    // Exact Match (Numbers, Dates, Booleans)
                    else
                    {
                        // Convert the string into an number
                        Type targetType = Nullable.GetUnderlyingType(prop.ClrType) ?? prop.ClrType;
                        object convertedValue = Convert.ChangeType(filter.Value, targetType);
                        
                        query = query.Where($"{prop.Name} == @0", convertedValue);
                    }
                }
                // Ignore it, if filter value not valid.
                catch { }
            }

            return query;
        }

        /// <summary>
        /// Applies sorting to the query based on the provided sortBy property and sortDescending flag.
        /// </summary>
        /// <param name="query"></param>
        /// <param name="sortBy"></param>
        /// <param name="sortDescending"></param>
        /// <returns></returns>
        private IQueryable<T> ApplySorting(IQueryable<T> query, string? sortBy, bool sortDescending)
        {
            if (!string.IsNullOrWhiteSpace(sortBy) && _entityProperties.TryGetValue(sortBy, out IProperty? sortProp))
            {
                string sortDirection = sortDescending ? "descending" : "ascending";
                return query.OrderBy($"{sortProp.Name} {sortDirection}");
            }
            
            // Fallback to sorting by 'Id' or the first property available
            string fallbackProp = _entityProperties.ContainsKey("Id") ? "Id" : _entityProperties.Values.FirstOrDefault()?.Name ?? "Id";
            return query.OrderBy($"{fallbackProp} ascending");
        }
    }
}