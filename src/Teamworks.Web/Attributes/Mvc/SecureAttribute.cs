using System;
using System.Web.Mvc;
using Raven.Client.Authorization;
using Teamworks.Web.Helpers.Extensions.Mvc;

namespace Teamworks.Web.Attributes.Mvc
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class SecureAttribute : ActionFilterAttribute
    {
        public SecureAttribute(string operation)
        {
            Operation = operation;
        }

        public string Operation { get; set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string id = filterContext.HttpContext.GetCurrentPersonId() ?? string.Empty;
            var session = filterContext.HttpContext.GetCurrentRavenSession();
            session.SecureFor(id, Operation);
        }
    }
}