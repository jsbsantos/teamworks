using System;
using System.Web.Mvc;
using Raven.Client.Authorization;
using Teamworks.Core.Services;
using Teamworks.Web.Helpers.Extensions.Mvc;

namespace Teamworks.Web.Attributes.Mvc
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class SecureForAttribute : ActionFilterAttribute
    {
        public SecureForAttribute()
        {
            Operation = Global.Constants.Operation;
        }


        public SecureForAttribute(string operation)
        {
            Operation = operation;
        }

        public string Operation { get; set; }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            string id = context.HttpContext.GetCurrentPersonId();
            if (!string.IsNullOrEmpty(id))
            {
                var session = context.HttpContext.GetCurrentRavenSession();
                session.SecureFor(id, Operation);
            }
            base.OnActionExecuting(context);
        }
    }
}