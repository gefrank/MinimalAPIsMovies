using Microsoft.EntityFrameworkCore;
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
        }

        // DbSet is a collection of entities that can be queried
        // The DbSet property is used to query and save instances of the Genre class   
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Actor> Actors { get; set; }
        public DbSet<Movie> Movies { get; set; }

    }
}
