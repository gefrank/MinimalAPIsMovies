﻿namespace MinimalAPIsMovies.Entities
{
    public class Movie
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public bool InTheaters { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string? Poster { get; set; }

        // creates one to many relationship between Movie and Comment
        public List<Comment> Comments { get; set; } = new List<Comment>();
        public List<GenreMovie> GenresMovies { get; set; } = new List<GenreMovie>();
        public List<ActorMovie> ActorsMovies { get; set; } = new List<ActorMovie>();
    }
}
