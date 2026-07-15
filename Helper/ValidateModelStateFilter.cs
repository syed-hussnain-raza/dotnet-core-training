using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MyAssignment.Constants;

namespace MyAssignment.Helper
{
    /// <summary>
    /// A filter that checks the ModelState before executing an action. 
    /// If the ModelState is invalid, it returns a BadRequest response 
    /// with the validation errors.
    /// </summary>
    public class ValidateModelStateFilter : IActionFilter
	{
		public void OnActionExecuting(ActionExecutingContext context)
		{
			if (!context.ModelState.IsValid)
			{
				var errors = context.ModelState
					.Where(ms => ms.Value != null && ms.Value.Errors.Count > 0)
					.ToDictionary(
						kvp => kvp.Key,
						kvp => kvp.Value!.Errors.Select(e => e.ErrorMessage).ToArray()
					);

				context.Result = new BadRequestObjectResult(
					Response<object>.FailureResponse(MessagesConstants.ValidationError, errors));
			}
		}

		public void OnActionExecuted(ActionExecutedContext context) { }
	}
}