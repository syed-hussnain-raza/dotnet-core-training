namespace MyAssignment.Helper
{
  // generic class for API Response
  public class ApiResponse<T>
  {
    // true if request succeeded, false if failed
    public bool Success {get; set;}
    // explanation of the result
    public string Message{get; set;} = string.Empty;
    
    // actual payload - null when nothing to return e.g. DELETE
    public T? Data {get; set;}

    // constructor
    public  ApiResponse (bool success, string message, T? data)
    {
      Success = success;
      Message = message;
      Data = data;
    }

    // success response with data
    public static ApiResponse<T> SuccessResponse(string message, T? data)
    {
        return new ApiResponse<T>(true, message, data);
    }

    // fail response - no data needed
    public static ApiResponse<T> FailResponse(string message)
    {
        return new ApiResponse<T>(false, message, default);
    }
  }
}