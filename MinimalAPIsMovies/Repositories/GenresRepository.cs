using Microsoft.EntityFrameworkCore;
using MinimalAPIsMovies.Entities;

namespace MinimalAPIsMovies.Repositories
{
    public class GenresRepository : IGenresRepository
    {
        private readonly ApplicationDbContext context;

        /// <summary>
        /// ApplicationDbContext is injected into the constructor of the GenresRepository class.
        /// </summary>
        /// <param name="context"></param>
        public GenresRepository(ApplicationDbContext context)
        {
                this.context = context; 
        }

        public async Task<int> Create(Genre genre)
        {
            context.Add(genre);
            await context.SaveChangesAsync();
            return genre.Id;
        }

        public async Task Delete(int id)
        {
             await context.Genres.Where(x => x.Id == id).ExecuteDeleteAsync();
        }

        public async Task<bool> Exists(int id)
        {
            return await context.Genres.AnyAsync(x => x.Id == id);
        }

        public async Task<List<Genre>> GetAll()
        {
            return await context.Genres.OrderBy(x => x.Name).ToListAsync();
        }

        public async Task<Genre?> GetById(int id)
        {
            return await context.Genres.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task Update(Genre genre)
        {
            context.Update(genre);
            await context.SaveChangesAsync();
        }
    }
}
