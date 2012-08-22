using System.Web.Mvc;
using Raven.Client;
using Teamworks.Web.Helpers.Extensions.Mvc;

namespace Teamworks.Web.Controllers
{
    [Authorize]
    public abstract class AppController : Controller
    {
        public IDocumentSession DbSession { get; set; }

        protected override void OnActionExecuting(ActionExecutingContext context)
        {
            DbSession = context.HttpContext.GetCurrentRavenSession();
            base.OnActionExecuting(context);
        }

        protected override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            if (filterContext.Result is ViewResult)
                ViewBag.Breadcrumb = CreateBreadcrumb();
            base.OnResultExecuting(filterContext);
        }

        protected override void OnResultExecuted(ResultExecutedContext context)
        {
            if ((context.Exception == null || context.ExceptionHandled) && DbSession != null)
            {
                using (var session = DbSession)
                {
                    session.SaveChanges();
                }
            }
            base.OnResultExecuted(context);
        }

        public virtual Breadcrumb[] CreateBreadcrumb()
        {
            return new Breadcrumb[0];
        }

        #region Nested type: Breadcrumb

        public class Breadcrumb
        {
            public string Url { get; set; }
            public string Name { get; set; }
        }

        #endregion
    }
}