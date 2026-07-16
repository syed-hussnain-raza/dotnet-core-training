namespace MyAssignment.Helper
{
    public enum ResponseStatus
    {
        Failure = 0,
        Success = 1
    }

    public class Response<T>
    {
        public ResponseStatus Status { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Payload { get; set; }
        public object? Errors { get; set; }

        public static Response<T> SuccessResponse(string message, T? payload)
        {
            return new Response<T>
            {
                Status = ResponseStatus.Success,
                Message = message,
                Payload = payload
            };
        }

        public static Response<T> FailureResponse(string message, object? errors = null)
        {
            return new Response<T>
            {
                Status = ResponseStatus.Failure,
                Message = message,
                Errors = errors
            };
        }
    }
}
