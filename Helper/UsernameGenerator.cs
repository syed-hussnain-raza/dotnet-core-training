namespace MyAssignment.Helper
{
    /// <summary>
    /// Utility for generating standardized usernames.
    /// </summary>
    public static class UsernameGenerator
    {
        /// <summary>
        /// Generates a username by combining the first and last name with a space.
        /// </summary>
        public static string Generate(string firstName, string lastName)
        {
            return $"{firstName} {lastName}";
        }
    }
}
