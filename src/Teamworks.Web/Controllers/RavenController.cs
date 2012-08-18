using System.Web.Mvc;
using Raven.Client;
using Teamworks.Web.Helpers.Extensions.Mvc;

namespace Teamworks.Web.Controllers
{
    [Authorize]
    public class RavenController : Controller
    {
        public IDocumentSession DbSession { get; set; }

        protected override void OnActionExecuting(ActionExecutingContext context)
        {
            DbSession = context.HttpContext.GetCurrentRavenSession();
            base.OnActionExecuting(context);
        }

        protected override void OnResultExecuted(ResultExecutedContext context)
        {
            if ((context.Exception == null || context.ExceptionHandled) && DbSession != null)
            {
                using (IDocumentSession session = DbSession)
                {
                    session.SaveChanges();
                }
            }
            base.OnResultExecuted(context);
        }
    }
}