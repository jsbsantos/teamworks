using System;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Teamworks.Web.Helpers.Extensions.Mvc;

namespace Teamworks.Web.Attributes.Mvc
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class VetoProjectAttribute : ActionFilterAttribute
    {
        public VetoProjectAttribute()
        {
            RouteValue = "projectId";
        }

        public string RouteValue { get; set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            int id;
            try
            {
                id = int.Parse(filterContext.RouteData.Values[RouteValue].ToString());
            }
            catch (Exception)
            {
                throw new HttpException((int)HttpStatusCode.NotFound, "Not Found");
            }
            var session = filterContext.HttpContext.GetCurrentRavenSession();
            session.Load<Core.Project>(id);
        }
    }
}