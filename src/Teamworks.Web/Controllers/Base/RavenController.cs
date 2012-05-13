using System.Linq;
using System.Security.Principal;
using System.Web.Mvc;
using System.Web.Routing;
using Raven.Client;
using Teamworks.Core;
using Teamworks.Core.Authentication;
using Teamworks.Core.People;

namespace Teamworks.Web.Controllers.Base
{
    [Authorize]
    public class RavenController : Controller
    {
        public IDocumentSession DbSession
        {
            get { return Global.Raven.CurrentSession; }
        }

        protected override void Initialize(RequestContext context)
        {
            Global.Raven.TryOpen();

            var identity = context.HttpContext.User.Identity;
            if (string.IsNullOrEmpty(identity.Name))
            {
                base.Initialize(context);
                return;
            }

            var person = DbSession.Load<Person>(identity.Name);
            if (person != null)
            {
                context.HttpContext.User = new GenericPrincipal(new PersonIdentity(person), person.Roles.ToArray());
            }

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