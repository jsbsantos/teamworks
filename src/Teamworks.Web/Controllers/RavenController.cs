using Raven.Client;
using System.Web.Mvc;
using Teamworks.Core.Services;

namespace Teamworks.Web.Controllers
{
    [Authorize]
    public class RavenController : Controller
    {
        public RavenController()
        {
        }

        public RavenController(IDocumentSession session)
        {
            DbSession = session;
        }

        public IDocumentSession DbSession { get; set; }

        protected override void OnActionExecuting(ActionExecutingContext context)
        {
            var session = context.HttpContext.Items[App.Keys.RavenDbSessionKey] as IDocumentSession;
            if (session == null)
            {
                session = Global.Database.OpenSession();
                context.HttpContext.Items[App.Keys.RavenDbSessionKey] = session;
            }
            DbSession = session;
            base.OnActionExecuting(context);
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
    }
}