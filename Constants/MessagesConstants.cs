namespace MyAssignment.Constants
{
    /// <summary>
    /// Centralized collection of response messages used across the User API.
    /// </summary>
    public static class MessagesConstants
    {
        // Api related messages
        public const string UserNotFound = "User not found.";
        public const string UserCreated = "User created successfully.";
        public const string UserDeleted = "User deleted successfully.";
        public const string UserUpdated = "User updated successfully.";
        public const string UserFetched = "User fetched successfully.";
        public const string UsersFetched = "Fetching all users.";

        // Validation related messages
        public const string InvalidEmailFormat = "Email format is invalid.";
        public const string InvalidPhoneFormat = "Phone number format is invalid.";
        public const string InvalidMembershipType = "MembershipType must be either 'Basic' or 'Premium'.";

        // Error related messages (general)
        public const string UnexpectedError = "An unexpected error occurred while processing the request.";
        public const string ValidationError = "Validation Failed";

        // Authentication related messages
        public const string InvalidCredentials = "Invalid email or password.";
        public const string UserRegistered = "Account registered successfully.";
        public const string RegistrationFailed = "Registration failed. Please ensure your information is valid and try again.";
        public const string LoginSuccess = "Login successful.";
    }
}