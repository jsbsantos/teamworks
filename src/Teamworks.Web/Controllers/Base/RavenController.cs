using System.Web.Mvc;
using System.Web.Routing;
using Raven.Client;
using Teamworks.Core;

namespace Teamworks.Web.Controllers.Base {
    public class RavenController : Controller {
        public IDocumentSession DbSession {
            get { return Global.Raven.CurrentSession; }
        }

        protected override void Initialize(RequestContext context) {
            Global.Raven.TryOpen();
            base.Initialize(context);
        }

        protected override void OnResultExecuted(ResultExecutedContext context) {
            if ((context.Exception == null || context.ExceptionHandled) && DbSession != null) {
                DbSession.SaveChanges();
            }
            base.OnResultExecuted(context);
        }
    }
}