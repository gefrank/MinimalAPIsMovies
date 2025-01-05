namespace MinimalAPIsMovies.DTOs
{
    // Hides the ID property, as it is not needed when creating a new genre
    public class CreateGenreDTO
    {
        public string Name { get; set; } = null!;
    }
}
