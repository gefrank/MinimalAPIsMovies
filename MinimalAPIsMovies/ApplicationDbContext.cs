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
            modelBuilder.Entity<Genre>().Property(genre => genre.Name).HasMaxLength(150);

            modelBuilder.Entity<Actor>().Property(actor => actor.Name).HasMaxLength(150);
            modelBuilder.Entity<Actor>().Property(actor => actor.Picture).IsUnicode(false);
        }

        // DbSet is a collection of entities that can be queried
        // The DbSet property is used to query and save instances of the Genre class   
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Actor> Actors { get; set; }

    }
}
