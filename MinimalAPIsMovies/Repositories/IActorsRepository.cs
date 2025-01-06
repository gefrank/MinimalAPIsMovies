using MinimalAPIsMovies.Entities;

namespace MinimalAPIsMovies.Repositories
{
    public interface IActorsRepository
    {
        Task<Actor> Create(Actor actor);
        Task Delete(int id);
        Task<bool> Exists(int id);
        Task<List<Actor>> GetAll();       
        Task<Actor?> GetById(int id);
        Task Update(Actor actor);
    }
}