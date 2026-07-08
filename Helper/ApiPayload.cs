namespace MyAssignment.Helper
{
    /// <summary>
    /// Interface for payloads that can be converted to ApiResponse.
    /// </summary>
    public interface IApiPayload
    {
        object ToApiResponse();
    }

    /// <summary>
    /// Generic wrapper for API responses, allowing for consistent response shapes.
    /// </summary>
    public class ApiPayload<T> : IApiPayload
    {
        public string Message { get; }
        public T? Data { get; }

        public ApiPayload(string message, T? data)
        {
            Message = message;
            Data = data;
        }

        /// <summary>
        /// Converts the ApiPayload to an ApiResponse object for API responses.
        /// </summary>
        /// <returns></returns>
        public object ToApiResponse()
        {
            return ApiResponse<T>.SuccessResponse(Message, Data);
        }
    }
}