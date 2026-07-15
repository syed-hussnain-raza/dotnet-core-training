using Microsoft.AspNetCore.Identity;

namespace MyAssignment.Models
{
    /// <summary>
    /// Represents a single User in the system.
    /// </summary>
    public class User : IdentityUser
    {
        /// <summary>
        /// First name of the user.
        /// </summary>
        public string FirstName { get; set; } = string.Empty;

        /// <summary>
        /// Last name of the user.
        /// </summary>
        public string LastName { get; set; } = string.Empty;

        /// <summary>
        /// Date of birth of the user.
        /// </summary>
        public DateOnly DateOfBirth { get; set; }

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
        public User(string firstName, string lastName, string email, string phoneNumber, DateOnly dateOfBirth, string address = "", string membershipType = "Basic", bool isActive = true)
        {
            UserName = firstName + lastName;
            Email = email;
            FirstName = firstName;
            LastName = lastName;
            PhoneNumber = phoneNumber;
            DateOfBirth = dateOfBirth;
            Address = address;
            MembershipType = membershipType;
            IsActive = isActive;
        }
    }
}