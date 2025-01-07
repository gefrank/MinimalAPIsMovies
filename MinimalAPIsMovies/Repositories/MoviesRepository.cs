using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MinimalAPIsMovies.DTOs;
using MinimalAPIsMovies.Entities;
using MinimalAPIsMovies.Migrations;
using System.Collections.Generic;

namespace MinimalAPIsMovies.Repositories
{
    public class MoviesRepository(IHttpContextAccessor httpContextAccessor,
                                  ApplicationDbContext context, 
                                  IMapper mapper) : IMoviesRepository
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
            return await context.Movies
                .Include(x => x.Comments)
                .AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
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
        public async Task Assign(int id, List<int> genresIds)
        {
            var movie = await context.Movies.Include(m => m.GenresMovies)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (movie is null)
            {
                throw new ArgumentException($"There's no movie with id {id}");
            }

            var genresMovies = genresIds.Select(genreId => new GenreMovie { GenreId = genreId });

            //  Use AutoMapper to map the new list of GenreMovie entities to the movie's existing GenresMovies collection. This effectively updates the movie's genres by:
            // 	Removing any genres that are not in the new list.
            //	Adding new genres that are in the new list.
            //	Updating existing genres.
            movie.GenresMovies = mapper.Map(genresMovies, movie.GenresMovies);

            await context.SaveChangesAsync();

        }

    }
}
