using System.Web.Routing;
using Raven.Client;
using System.Web.Mvc;
using Teamworks.Core.Services;
using Teamworks.Web.Helpers.Mvc;

namespace Teamworks.Web.Controllers
{
    [Authorize]
    public class RavenDbController : Controller
    {
        public RavenDbController()
        {
            
        }

        public RavenDbController(IDocumentSession session)
        {
            DbSession = session;
        }

        private IDocumentSession _session;
        protected IDocumentSession DbSession
        {
            set { _session = value; }
            get
            {
                return _session ?? (_session = HttpContext.GetOrOpenCurrentSession());
            }
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