using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyAssignment.Constants;
using MyAssignment.Models;

namespace MyAssignment.Data.Configurations
{
    /// <summary>
    /// EF Core schema configuration for the User entity.
    /// </summary>
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        /// <summary>
        /// Configures the User entity's schema using Fluent API. This includes setting
        /// primary keys, required fields, maximum lengths, unique constraints, and default values.
        /// </summary>
        /// <param name="entity"></param>
        public void Configure(EntityTypeBuilder<User> entity)
        {
            // Set the primary key for the User entity.
            entity.HasKey(u => u.Id);

            // Configure the FirstName property to be required and have a maximum length of 50 characters.
            entity.Property(u => u.FirstName)
                .IsRequired()
                .HasMaxLength(50);

            // Configure the LastName property to be required and have a maximum length of 50 characters.
            entity.Property(u => u.LastName)
                .IsRequired()
                .HasMaxLength(50);

            // Configure the Email property to be required, have a maximum length of 256 characters, and be unique.
            entity.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(256);

            // Create a unique index on the Email property to enforce uniqueness at the database level.
            entity.HasIndex(u => u.Email)
                .IsUnique();

            // Configure the PhoneNumber property to be required and have a maximum length of 20 characters.
            entity.Property(u => u.PhoneNumber)
                .IsRequired()
                .HasMaxLength(20);

            // Configure Address property
            entity.Property(u => u.Address)
                .HasMaxLength(500);

            // Configure the MembershipType property to be required, have a maximum length of 20 characters,
            // and have a default value of "Basic".
            entity.Property(u => u.MembershipType)
                .IsRequired()
                .HasMaxLength(20)
                .HasDefaultValue(MembershipTypesConstants.Basic);

            // Configure the IsActive property to have a default value of true.
            entity.Property(u => u.IsActive)
                .HasDefaultValue(true);
        }
    }
}