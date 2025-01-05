using Microsoft.EntityFrameworkCore;
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

        }

        // DbSet is a collection of entities that can be queried
        // The DbSet property is used to query and save instances of the Genre class   
        public DbSet<Genre> Genres { get; set; }

    }
}
