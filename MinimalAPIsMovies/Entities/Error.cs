namespace MinimalAPIsMovies.Entities
{
    public class Error
    {
        public Guid id { get; set; }
        public string ErrorMessage { get; set; } = null!;
        public string? StackTrace { get; set; }
        public DateTime Date { get; set; } 
    }
}

