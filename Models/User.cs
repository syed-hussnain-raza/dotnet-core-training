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
        /// First name of the user.
        /// </summary>
        public string FirstName { get; set; } = string.Empty;

        /// <summary>
        /// Last name of the user.
        /// </summary>
        public string LastName { get; set; } = string.Empty;

        /// <summary>
        /// Email address of the user.
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Contact phone number of the user.
        /// </summary>
        public string PhoneNumber { get; set; } = string.Empty;

        /// <summary>
        /// Date of birth of the user.
        /// </summary>
        public DateTime? DateOfBirth { get; set; }

        /// <summary>
        /// Address of the user.
        /// </summary>
        public string Address { get; set; } = string.Empty;

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
        public User(int id, string firstName, string lastName, string email, string phoneNumber, DateTime? dateOfBirth = null, string address = "", string membershipType = "Basic", bool isActive = true)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            PhoneNumber = phoneNumber;
            DateOfBirth = dateOfBirth;
            Address = address;
            MembershipType = membershipType;
            IsActive = isActive;
        }
    }
}