using System.ComponentModel.DataAnnotations;
using MyAssignment.Constants;

namespace MyAssignment.Dtos
{
    /// <summary>
    /// Data transfer object used for creating and updating a user via the API.
    /// </summary>
    public class UserDto
    {
        /// <summary>
        /// FullName is required and cannot be empty.
        /// </summary>
        [Required]
        public string FullName { get; set; } = string.Empty;

        /// <summary>
        /// Email is required and must follow a valid email format.
        /// </summary>
        [Required]
        [EmailAddress(ErrorMessage = MessagesConstants.InvalidEmailFormat)]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// PhoneNumber is required and must follow valid phone number format (e.g., 03XXXXXXXXX).
        /// </summary>
        [Required]
        [RegularExpression(@"^03\d{9}$", ErrorMessage = MessagesConstants.InvalidPhoneFormat)]
        public string PhoneNumber { get; set; } = string.Empty;

        /// <summary>
        /// MembershipType can be either "Basic" or "Premium" only.
        /// </summary>
        [RegularExpression("^(Basic|Premium)$", ErrorMessage = MessagesConstants.InvalidMembershipType)]
        public string MembershipType { get; set; } = "Basic";
    }
}