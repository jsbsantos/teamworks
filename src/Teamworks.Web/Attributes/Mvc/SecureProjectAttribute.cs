using System;
using System.Net;
using System.Web;
using System.Web.Mvc;
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
                // todo better error handling
                throw new HttpException((int)HttpStatusCode.NotFound, "Not Found");
            }

            var session = filterContext.HttpContext.GetCurrentRavenSession();
            session.Load<Core.Project>(id);
        }
    }
}