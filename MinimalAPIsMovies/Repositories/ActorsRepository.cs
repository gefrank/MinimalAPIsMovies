using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using MinimalAPIsMovies.DTOs;
using MinimalAPIsMovies.Entities;

namespace MinimalAPIsMovies.Repositories
{
    // IHttpContextAccessor allows you to access the current HttpContext in any class
    public class ActorsRepository(ApplicationDbContext context, 
                                  IHttpContextAccessor httpContextAccessor) : IActorsRepository
    {
        public async Task<List<Actor>> GetAll(PaginationDTO pagination)
        {
            var queryable = context.Actors.AsQueryable();
            await httpContextAccessor.HttpContext!.InsertPaginationInResponseHeader(queryable);
            return await queryable.OrderBy(x => x.Name).Paginate(pagination).ToListAsync();
        }

        public async Task<Actor?> GetById(int id)
        {
            // AsNoTracking() is used to avoid tracking the entity in the context
            return await context.Actors.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<Actor>> GetByName(string name)
        {
            return await context.Actors
                            .Where(x => x.Name.Contains(name))
                            .OrderBy(x => x.Name).ToListAsync();
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
