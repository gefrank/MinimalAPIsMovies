using MinimalAPIsMovies.Entities;

namespace MinimalAPIsMovies.Repositories
{
    /// <summary>
    ///  This follows the repository pattern, which is a design pattern that isolates the data access logic in your application.
    /// </summary>
    public interface IGenresRepository
    {
        Task<int> Create(Genre genre);
        Task<Genre?> GetById(int id);
        Task<List<Genre>> GetAll();
        Task<bool> Exists(int id);
        Task Update(Genre genre);
        Task Delete(int id);

    }
}
