namespace MyAssignment.Constants
{
    /// <summary>
    /// Centralized allowed values for User.MembershipType. Kept as a shared
    /// reference for seed data and any future lookups; the DTO's regex
    /// validation is the actual enforcement point now (see UserDto).
    /// </summary>
    public static class MembershipTypes
    {
        public const string Basic = "Basic";
        public const string Premium = "Premium";

        public static readonly string[] AllowedTypes =
        {
          Basic,
          Premium
        };
    }
}