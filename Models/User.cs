using System.ComponentModel.DataAnnotations;
using MyAssignment.Constants;

namespace MyAssignment.Models
{
    /// <summary>
    /// Represents a single User in the system.
    /// </summary>
    public class User
    {
        /// <summary>
        /// Unique identifier of the user. Server-generated.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Full name of the user.
        /// </summary>
        public string FullName { get; set; } = string.Empty;

        /// <summary>
        /// Email address of the user.
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Contact phone number of the user.
        /// </summary>
        public string PhoneNumber { get; set; } = string.Empty;

        /// <summary>
        /// Membership tier of the user. Expected values: "Basic" or "Premium".
        /// </summary>
        public string MembershipType { get; set; } = "Basic";

        /// <summary>
        /// Indicates whether the user's account is active.
        /// </summary>
        public bool IsActive { get; set; } = true;
        
        /// <summary>
        /// Parameters less constructor
        /// </summary>
        public User ()
        { }

        /// <summary>
        /// Creates a new User with all fields.
        /// </summary>
        public User(int id, string fullName, string email, string phoneNumber, string membershipType = "Basic", bool isActive = true)
        {
            Id = id;
            FullName = fullName;
            Email = email;
            PhoneNumber = phoneNumber;
            MembershipType = membershipType;
            IsActive = isActive;
        }
    }
}