using MinimalAPIsMovies.DTOs;
using System.Linq;
using System.Reflection.Metadata.Ecma335;

namespace MinimalAPIsMovies.Repositories
{
    // Centralize the pagination logic in a single place so that it can be reused
    public static class IQueryableExtensions
    {
        public static IQueryable<T> Paginate<T>(this IQueryable<T> queryable, PaginationDTO pagination)
        {
            return queryable.Skip((pagination.Page - 1) * pagination.RecordsPerPage).Take(pagination.RecordsPerPage);
        }
    }
}
