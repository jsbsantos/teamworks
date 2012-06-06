using System.Linq;
using System.Security.Principal;
using System.Web.Mvc;
using System.Web.Routing;
using Raven.Client;
using Teamworks.Core.Authentication;
using Teamworks.Core.People;
using Teamworks.Core.Services;
using Teamworks.Web.Helpers.Extensions;

namespace Teamworks.Web.Controllers
{
    [Authorize]
    public class RavenController : Controller
    {
        public IDocumentSession DbSession
        {
            get { return Global.Raven.CurrentSession; }
        }

        protected override void OnResultExecuted(ResultExecutedContext context)
        {
            if ((context.Exception == null || context.ExceptionHandled) && DbSession != null)
            {
                using (IDocumentSession session = DbSession)
                {
                    DbSession.SaveChanges();
                }
            }
            base.OnResultExecuted(context);
        }
    }
}