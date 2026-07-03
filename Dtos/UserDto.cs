using System.ComponentModel.DataAnnotations;
using MyAssignment.Constants;

namespace MyAssignment.Dtos
{
    /// <summary>
    /// Data transfer object used for creating and updating a user via the API.
    /// Validation is enforced declaratively via DataAnnotations and checked
    /// against ModelState in the controller.
    /// </summary>
    public class UserDto
    {
        [Required(ErrorMessage = UserMessages.ValidationFailed)]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = UserMessages.ValidationFailed)]
        [EmailAddress(ErrorMessage = UserMessages.InvalidEmailFormat)]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = UserMessages.ValidationFailed)]
        [RegularExpression(@"^03\d{9}$", ErrorMessage = UserMessages.InvalidPhoneFormat)]
        public string PhoneNumber { get; set; } = string.Empty;

        // Keep this pattern in sync with MembershipTypes.AllowedTypes.
        [RegularExpression("^(Basic|Premium)$", ErrorMessage = UserMessages.InvalidMembershipType)]
        public string MembershipType { get; set; } = "Basic";
    }
}