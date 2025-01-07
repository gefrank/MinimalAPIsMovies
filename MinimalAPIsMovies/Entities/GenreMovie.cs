namespace MinimalAPIsMovies.Entities
{
    public class GenreMovie
    {
        public int MovieId { get; set; }
        public int GenreId { get; set; }
        // Navigation properties, tell us about the relationship between the entities
        public Movie Movie { get; set; } = null!;
        public Genre Genre { get; set; } = null!;
    }
}
