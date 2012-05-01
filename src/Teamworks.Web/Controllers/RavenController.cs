using System.Web.Mvc;
using Raven.Client;
using Teamworks.Core.Extensions;
using Global = Teamworks.Web.Models.Global;

namespace Teamworks.Web.Controllers
{
    public class RavenController : Controller
    {
        public IDocumentSession DbSession
        {
            get { return Local.Data[Global.RavenKey] as IDocumentSession; }
        }

        protected override void Initialize(System.Web.Routing.RequestContext context)
        {
            var session = Global.DocumentStore.OpenSession();
            Local.Data[Global.RavenKey] = session;
            base.Initialize(context);
        }

        protected override void OnResultExecuted(ResultExecutedContext context)
        {
            if ((context.Exception == null || context.ExceptionHandled) && DbSession != null)
            {
                DbSession.SaveChanges();
            }
            base.OnResultExecuted(context);
        }
    }
}

