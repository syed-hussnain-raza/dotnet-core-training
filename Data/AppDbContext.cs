using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using MyAssignment.Models;

namespace MyAssignment.Data
{
    /// <summary>
    /// EF Core database context for the application. Inherits from
    /// IdentityDbContext to add ASP.NET Core Identity's own tables
    /// </summary>
    public class AppDbContext : IdentityDbContext<User>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AppDbContext"/> class.
        /// </summary>
        /// <param name="options">
        /// The options used to configure the database context.
        /// </param>
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        /// <summary>
        /// Applies all entity configurations from the current assembly.
        /// </summary>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure ASP.NET Core Identity tables.
            base.OnModelCreating(modelBuilder);

            // Apply all entity configurations from the assembly containing AppDbContext.
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }
    }
}