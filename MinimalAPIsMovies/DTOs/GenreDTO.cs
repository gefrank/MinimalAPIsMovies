namespace MinimalAPIsMovies.DTOs
{
    /// <summary>
    /// DTOs allow your entities to evolve without affecting the contract of your API.
    /// </summary>
    public class GenreDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
    }
}
