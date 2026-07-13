using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace MyAssignment.Helper
{
    public static class QueryableExtensions
    {
        public static async Task<(List<T> Items, int TotalCount)> ToPagedResultAsync<T>(
            this IQueryable<T> query,
            int page,
            int pageSize,
            Expression<Func<T, bool>>? filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null)
        {
            if (filter != null)
            {
                query = query.Where(filter);
            }

            int totalCount = await query.CountAsync();

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            var items = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            return (items, totalCount);
        }
    }
}
