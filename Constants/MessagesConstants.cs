namespace MyAssignment.Constants
{
    /// <summary>
    /// Centralized collection of response messages used across the User API.
    /// </summary>
    public static class MessagesConstants
    {
        public const string UserNotFound = "User not found.";
        public const string InvalidEmailFormat = "Email format is invalid.";
        public const string InvalidPhoneFormat = "Phone number format is invalid.";
        public const string InvalidMembershipType = "MembershipType must be either 'Basic' or 'Premium'.";
        public const string UserDeleted = "User deleted successfully.";
        public const string UserUpdated = "User updated successfully.";
        public const string UnexpectedError = "An unexpected error occurred while processing the request.";
    }
}