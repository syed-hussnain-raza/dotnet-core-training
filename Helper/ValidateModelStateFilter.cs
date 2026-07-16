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
				context.Result = new BadRequestObjectResult(
					Response<object>.FailureResponse(MessagesConstants.ValidationError));
			}
		}

		public void OnActionExecuted(ActionExecutedContext context) { }
	}
}