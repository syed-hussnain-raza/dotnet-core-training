using Microsoft.EntityFrameworkCore;
using MyAssignment.Models;

namespace MyAssignment.Data
{
    /// <summary>
    /// EF Core database context for the application.
    /// </summary>
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; } = null!;

        /// <summary>
        /// Applies all entity configurations from the current assembly.
        /// </summary>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }
    }
}