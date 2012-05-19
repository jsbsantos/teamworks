using System.Linq;
using System.Security.Principal;
using System.Web.Mvc;
using System.Web.Routing;
using Raven.Client;
using Teamworks.Core;
using Teamworks.Core.Authentication;
using Teamworks.Core.People;
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

        protected override void Initialize(RequestContext context)
        {
            Global.Raven.TryOpen();
            string id = context.HttpContext.User.Identity.Name;
            if (string.IsNullOrEmpty(id))
            {
                base.Initialize(context);
                return;
            }

            var person = DbSession.Load<Person>(id);
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
                using (var session = DbSession)
                {
                    DbSession.SaveChanges();    
                }
            }
            var person = context.HttpContext.GetCurrentPerson();
            context.HttpContext.User = new GenericPrincipal(new GenericIdentity(person.Id), new string[0]);
            base.OnResultExecuted(context);
        }
    }
}