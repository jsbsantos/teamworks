using Raven.Client;
using System.Web.Mvc;
using Teamworks.Core.Services;

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