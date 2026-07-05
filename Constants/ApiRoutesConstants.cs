namespace MyAssignment.Constants
{
    /// <summary>
    /// Defines API route constants.
    /// </summary>
    public static class ApiRoutesConstants
    {
        public const string Version = "v{version:apiVersion}";
        public const string Users = $"api/{Version}/users";
    }
}