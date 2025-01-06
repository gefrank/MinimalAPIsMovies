namespace MinimalAPIsMovies.Services
{
    public interface IFileStorage
    {
        Task<string> Store(string container, IFormFile file);
        Task Delete(string? route, string container);
        async Task<string> Edit(string? route, string container, IFormFile file)
        {
            // Substuting one file for another
            await Delete(route, container);
            return await Store(container, file);
        }
    }
}
