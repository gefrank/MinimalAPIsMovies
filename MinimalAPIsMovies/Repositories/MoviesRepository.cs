using Microsoft.EntityFrameworkCore;
using MinimalAPIsMovies.DTOs;
using MinimalAPIsMovies.Entities;

namespace MinimalAPIsMovies.Repositories
{
    public class MoviesRepository(IHttpContextAccessor httpContextAccessor,
                                  ApplicationDbContext context) : IMoviesRepository
    {
        public async Task<List<Movie>> GetAll(PaginationDTO pagination)
        {
            var queryable = context.Movies.AsQueryable();
            // This writes the total amount of records in the header of the response of http request
            await httpContextAccessor.HttpContext!.InsertPaginationInResponseHeader(queryable);
            return await queryable.OrderBy(x => x.Title)
                                  .Paginate(pagination)
                                  .ToListAsync();

        }

        public async Task<Movie?> GetById(int id)
        {
            return await context.Movies.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<bool>Exists(int id)
        {
            return await context.Movies.AnyAsync(x => x.Id == id);
        }

        public async Task<int> Create(Movie movie)
        {
            context.Add(movie);
            await context.SaveChangesAsync();
            return movie.Id;
        }
        public async Task Update(Movie movie)
        {
            context.Update(movie);
            await context.SaveChangesAsync();
        }
        public async Task Delete(int id)
        {
            await context.Movies.Where(x => x.Id == id).ExecuteDeleteAsync();
        }

    }
}
