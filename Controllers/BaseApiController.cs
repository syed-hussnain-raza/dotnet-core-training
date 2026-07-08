using Microsoft.AspNetCore.Mvc;
using MyAssignment.Helper;

namespace MyAssignment.Controllers
{
    /// <summary>
    /// Base controller for API endpoints, providing standardized response handling.
    /// </summary>
    public abstract class BaseApiController : ControllerBase
    {
        /// <summary>
        /// Returns 200 OK with the given message and data wrapped in a successful ApiResponse;.
        /// </summary>
        protected OkObjectResult Ok<T>(string message, T? data)
        {
            return (OkObjectResult)Ok((object?)new ApiPayload<T>(message, data));
        }

        /// <summary>
        /// Returns 400 Bad Request with the given message wrapped in a failed ApiResponse.
        /// </summary>
        protected BadRequestObjectResult BadRequest(string message)
        {
            return (BadRequestObjectResult)BadRequest((object?)message);
        }

        /// <summary>
        /// Underlying override: wraps any value in a successful ApiResponse.
        /// </summary>
        public override OkObjectResult Ok(object? value) =>
            base.Ok(
                value is IApiPayload payload
                    ? payload.ToApiResponse()
                    : ApiResponse<object>.SuccessResponse(string.Empty, value)
            );

        /// <summary>
        /// Underlying override: wraps any error in a failed ApiResponse.
        /// </summary>
        public override BadRequestObjectResult BadRequest(object? error) =>
            base.BadRequest(
                ApiResponse<object>.FailResponse(
                    error as string ?? error?.ToString() ?? string.Empty
                )
            );
    }
}