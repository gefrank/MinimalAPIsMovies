namespace MinimalAPIsMovies.DTOs
{
    public class CreateActorDTO
    {
        public string Name { get; set; } = null!;
        public DateTime DateOfBirth { get; set; }
        // IFormFile is a class that represents a file sent with the HttpRequest, it is used to receive files in the API
        // This is used to receive the picture of the actor from the client
        public IFormFile? Picture { get; set; }
    }
}
