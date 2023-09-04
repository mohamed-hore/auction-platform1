using System;
using System.Web.Mvc;

public class VerifyExecutionFilterAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext filterContext)
    {
        // Check if the previous action has been executed by verifying some condition
        bool previousActionExecuted = true;

        if (!previousActionExecuted)
        {
            // Redirect or handle the situation as needed
            filterContext.Result = new RedirectResult("~/ProductsController/Form"); // Redirect to the desired action
        }

        base.OnActionExecuting(filterContext);
    }
}
