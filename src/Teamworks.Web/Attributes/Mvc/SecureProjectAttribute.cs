using System;
using System.Web.Mvc;
using Teamworks.Core;
using Teamworks.Web.Helpers.Extensions.Mvc;

namespace Teamworks.Web.Attributes.Mvc
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class SecureProjectAttribute : SecureAttribute
    {
        public SecureProjectAttribute(string operation, string key = "projectId")
            : base(operation)
        {
            Key = key;
        }

        public string Key { get; set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            int id;
            try
            {
                id = int.Parse(filterContext.RouteData.Values[Key].ToString());
            }
            catch (Exception e)
            {
                filterContext.Result = new HttpNotFoundResult();
                return;
            }

            var session = filterContext.HttpContext.GetCurrentRavenSession();
            var project = session.Load<Project>(id);
            if (project == null)
                filterContext.Result = new HttpNotFoundResult();
        }
    }
}