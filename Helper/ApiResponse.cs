namespace MyAssignment.Helper
{
    /// <summary>
    /// Generic wrapper for all API responses, for consistence responses
    /// </summary>
    public class ApiResponse<T>
    {
        /// <summary>True if the request succeeded, false if it failed.</summary>
        public bool Success { get; set; }

        /// <summary>Human-readable explanation of the result.</summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>Actual payload — null when nothing to return (e.g. DELETE).</summary>
        public T? Data { get; set; }

        /// <summary>
        /// Creates a new ApiResponse.
        /// </summary>
        public ApiResponse(bool success, string message, T? data)
        {
            Success = success;
            Message = message;
            Data = data;
        }

        /// <summary>
        /// Builds a success response with data.
        /// </summary>
        public static ApiResponse<T> SuccessResponse(string message, T? data)
        {
            return new ApiResponse<T>(true, message, data);
        }

        /// <summary>
        /// Builds a failure response — no data needed.
        /// </summary>
        public static ApiResponse<T> FailResponse(string message)
        {
            return new ApiResponse<T>(false, message, default);
        }
    }
}