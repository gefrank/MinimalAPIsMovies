﻿using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Abstractions;
using MinimalAPIsMovies.Entities;

namespace MinimalAPIsMovies
{
    public class ApplicationDbContext: DbContext
    {
        // This will receive the DbContextOptions from the constructor of the Startup class
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure fluent API rules here
            modelBuilder.Entity<Genre>().Property(x => x.Name).HasMaxLength(150);

            modelBuilder.Entity<Actor>().Property(x => x.Name).HasMaxLength(150);
            modelBuilder.Entity<Actor>().Property(x => x.Picture).IsUnicode(false);

            modelBuilder.Entity<Movie>().Property(x => x.Title).HasMaxLength(250);
            modelBuilder.Entity<Movie>().Property(x => x.Poster).IsUnicode(false);

            // define the composite key for the GenreMovie table
            modelBuilder.Entity<GenreMovie>().HasKey(x => new { x.GenreId, x.MovieId });
            modelBuilder.Entity<ActorMovie>().HasKey(x => new { x.ActorId, x.MovieId });
        }

        // DbSet is a collection of entities that can be queried
        // The DbSet property is used to query and save instances of the Genre class   
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Actor> Actors { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<GenreMovie> GenreMovies { get; set; }
        public DbSet<ActorMovie> ActorMovies { get; set; }
        public DbSet<Error> Errors { get; set; }

    }
}
