using Microsoft.AspNetCore.Mvc;
using MyAssignment.Helper;

namespace MyAssignment.Shared

{
    /// <summary>
    /// Base controller for API endpoints, providing standardized response handling.
    /// </summary>
    public abstract class BaseApiController : ControllerBase
    {
        /// <summary>
        /// Returns 200 OK with the given message and data wrapped in a successful Response.
        /// </summary>
        protected OkObjectResult Ok<T>(string message, T? data)
        {
            return (OkObjectResult)Ok(Response<T>.SuccessResponse(message, data));
        }

        /// <summary>
        /// Returns 400 Bad Request with the given message wrapped in a failed Response.
        /// </summary>
        protected BadRequestObjectResult BadRequest(string message)
        {
            return (BadRequestObjectResult)BadRequest(Response<object>.FailureResponse(message));
        }

        /// <summary>
        /// Underlying override: wraps any value in a successful Response.
        /// </summary>
        public override OkObjectResult Ok(object? value)
        {
            OkObjectResult result;

            if (value != null && value.GetType().IsGenericType && value.GetType().GetGenericTypeDefinition() == typeof(Response<>))
            {
                result = base.Ok(value);
            }
            else
            {
                result = base.Ok(Response<object>.SuccessResponse(string.Empty, value));
            }

            return result;
        }

        /// <summary>
        /// Underlying override: wraps any error in a failed Response.
        /// </summary>
        public override BadRequestObjectResult BadRequest(object? error)
        {
            BadRequestObjectResult result;

            if (error != null && error.GetType().IsGenericType && error.GetType().GetGenericTypeDefinition() == typeof(Response<>))
            {
                result = base.BadRequest(error);
            }
            else
            {
                string message = error as string ?? error?.ToString() ?? string.Empty;
                result = base.BadRequest(Response<object>.FailureResponse(message));
            }

            return result;
        }
    }
}