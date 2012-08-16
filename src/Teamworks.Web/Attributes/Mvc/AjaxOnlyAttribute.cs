using System;
using System.Web.Mvc;

namespace Teamworks.Web.Attributes.Mvc
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class AjaxOnlyAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.HttpContext.Request.IsAjaxRequest())
            {
                context.Result = new HttpNotFoundResult("Only Ajax calls are permitted");
            }
        }
    }
}