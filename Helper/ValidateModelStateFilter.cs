using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

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
				string errors = string.Join(" ", context.ModelState.Values
					.SelectMany(v => v.Errors)
					.Select(e => e.ErrorMessage));

				context.Result = new BadRequestObjectResult(
					ApiResponse<object>.FailResponse(errors));
			}
		}

		public void OnActionExecuted(ActionExecutedContext context) { }
	}
}