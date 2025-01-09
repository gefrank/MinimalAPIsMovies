namespace MinimalAPIsMovies.DTOs
{
    public class EditClaimDTO
    {
        // If am an admin, I want to make another user an admin
        public string Email { get; set; } = null!;
    }
}
