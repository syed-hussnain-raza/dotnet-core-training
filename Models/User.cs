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
        [Required]
        public string FullName { get; set; } = string.Empty;

        /// <summary>
        /// Email address of the user.
        /// </summary>
        [Required]
        [EmailAddress(ErrorMessage = MessagesConstants.InvalidEmailFormat)]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Contact phone number of the user.
        /// </summary>
        [Required]
        [RegularExpression(@"^03\d{9}$", ErrorMessage = MessagesConstants.InvalidPhoneFormat)]
        public string PhoneNumber { get; set; } = string.Empty;

        /// <summary>
        /// Membership tier of the user. Expected values: "Basic" or "Premium".
        /// </summary>
        [Required]
        [RegularExpression(@"^(Basic|Premium)$", ErrorMessage = MessagesConstants.InvalidMembershipType)]
        public string MembershipType { get; set; } = "Basic";

        /// <summary>
        /// Indicates whether the user's account is active.
        /// </summary>
        public bool IsActive { get; set; } = true;

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