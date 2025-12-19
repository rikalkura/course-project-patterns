using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace OnlineStore.Web.Filters;

/// <summary>
/// Action filter that redirects admin users to the admin portal
/// </summary>
public class RedirectAdminAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (context.HttpContext.User.IsInRole("Admin"))
        {
            context.Result = new RedirectToActionResult("Index", "AdminProducts", null);
        }
        else
        {
            base.OnActionExecuting(context);
        }
    }
}

