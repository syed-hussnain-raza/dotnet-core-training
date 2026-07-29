using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace MyAssignment.Helper
{
    public static class PaginationHelper
    {
        /// <summary>
        /// Applies optional filtering and ordering to a query, then returns a paginated result
        /// along with the total number of matching records.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the query.</typeparam>
        /// <param name="query">The source query to paginate.</param>
        /// <param name="page">The 1-based page number to retrieve.</param>
        /// <param name="pageSize">The number of items to include in each page.</param>
        /// <param name="filter">An optional filter expression applied before counting and pagination.</param>
        /// <param name="orderBy">An optional ordering function applied before pagination.</param>
        /// <returns></returns>
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
