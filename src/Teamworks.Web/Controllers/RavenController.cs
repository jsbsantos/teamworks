using System.Web.Mvc;
using System.Web.Routing;
using Raven.Client;
using Teamworks.Core.Extensions;
using Global = Teamworks.Web.Models.Global;

namespace Teamworks.Web.Controllers {
    public class RavenController : Controller {
        public IDocumentSession DbSession {
            get { return Local.Data[Core.Extensions.Global.RavenKey] as IDocumentSession; }
        }

        protected override void Initialize(RequestContext context) {
            IDocumentSession session = Global.DocumentStore.OpenSession();
            Local.Data[Core.Extensions.Global.RavenKey] = session;
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