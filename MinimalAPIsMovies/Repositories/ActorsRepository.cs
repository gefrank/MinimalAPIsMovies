using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using MinimalAPIsMovies.Entities;

namespace MinimalAPIsMovies.Repositories
{
    public class ActorsRepository(ApplicationDbContext context) : IActorsRepository
    {
        public async Task<List<Actor>> GetAll()
        {
            return await context.Actors.OrderBy(x => x.Name).ToListAsync();
        }

        public async Task<Actor?> GetById(int id)
        {
            // AsNoTracking() is used to avoid tracking the entity in the context
            return await context.Actors.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Actor> Create(Actor actor)
        {
            context.Actors.Add(actor);
            await context.SaveChangesAsync();
            return actor;
        }

        public async Task<bool> Exists(int id)
        {
            return await context.Actors.AnyAsync(x => x.Id == id);
        }

        public async Task Update(Actor actor)
        {
            context.Actors.Update(actor);
            await context.SaveChangesAsync();       
        }

        public async Task Delete(int id)
        {
            await context.Actors.Where(x=> x.Id == id).ExecuteDeleteAsync();
        }

    }
}
