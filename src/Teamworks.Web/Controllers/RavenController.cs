using System.Web.Routing;
using Raven.Client;
using System.Web.Mvc;
using Teamworks.Core.Services;
using Teamworks.Web.Helpers.Mvc;

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

        protected IDocumentSession DbSession { get; set; }

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