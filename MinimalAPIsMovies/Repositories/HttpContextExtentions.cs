using Microsoft.EntityFrameworkCore;

namespace MinimalAPIsMovies.Repositories
{
    public static class HttpContextExtentions
    {
        // IQueryable is a generic interface that represents a collection of objects that can be queried
        public async static Task InsertPaginationInResponseHeader<T>(this HttpContext httpContext, IQueryable<T> queryable)
        {
            if (httpContext is null)
            {
                throw new ArgumentNullException(nameof(httpContext));
            }

            // How many records are in the database table
            double count = await queryable.CountAsync();
            httpContext.Response.Headers.Add("totalAmountOfRecords", count.ToString());



        }
    }
}
