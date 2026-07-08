using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyAssignment.Constants;
using MyAssignment.Models;

namespace MyAssignment.Data.Configurations
{
    /// <summary>
    /// Configures the User entity using the Fluent API.
    /// </summary>
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(u => u.Id);

            builder.Property(u => u.FullName)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(256);

            builder.HasIndex(u => u.Email)
                .IsUnique();

            builder.Property(u => u.PhoneNumber)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(u => u.MembershipType)
                .IsRequired()
                .HasMaxLength(20)
                .HasDefaultValue(MembershipTypesConstants.Basic);

            builder.Property(u => u.IsActive)
                .HasDefaultValue(true);
        }
    }
}