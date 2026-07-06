using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyAssignment.Models;
using MyAssignment.Constants;

namespace MyAssignment.Data
{
    /// <summary>
    /// EF Core database context for the application. Inherits from
    /// IdentityDbContext to add ASP.NET Core Identity's own tables
    /// </summary>
    public class AppDbContext : IdentityDbContext<IdentityUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public new DbSet<User> Users { get; set; } = null!;

        /// <summary>
        /// Configures schema-level constraints via Fluent API. These are enforced by SQL Server itself.
        /// </summary>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Required: IdentityDbContext's own OnModelCreating configures its Identity tables.
            base.OnModelCreating(modelBuilder);
            
            /// <summary>
            /// Configure the User entity
            /// </summary>
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.Id);

                entity.Property(u => u.FullName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(u => u.Email)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.HasIndex(u => u.Email)
                    .IsUnique();

                entity.Property(u => u.PhoneNumber)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(u => u.MembershipType)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasDefaultValue(MembershipTypesConstants.Basic);

                entity.Property(u => u.IsActive)
                    .HasDefaultValue(true);
            });
        }
    }
}