using System.Linq;
using System.Security.Principal;
using System.Web.Mvc;
using Raven.Client;
using Teamworks.Core;
using Teamworks.Core.Authentication;
using Teamworks.Core.Services;
using Teamworks.Web.Helpers.Mvc;

namespace Teamworks.Web.Attributes.Mvc
{
    public class FormsAuthenticationAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var user = context.HttpContext.User;
            if (user.Identity.IsAuthenticated)
            {
                var id = user.Identity.Name;
                if (!string.IsNullOrEmpty(id))
                {
                    var session = context.HttpContext.Items[App.Keys.RavenDbSessionKey] as IDocumentSession;
                    if (session == null)
                    {
                        session = Global.Database.OpenSession();
                        context.HttpContext.Items[App.Keys.RavenDbSessionKey] = session;
                    }

                    var person = session.Load<Person>(id);
                    if (person != null)
                    {
                        context.HttpContext.User = new GenericPrincipal(new PersonIdentity(person),
                                                                        person.Roles.ToArray());
                    }
                }
            }
            base.OnActionExecuting(context);
        }
    }
}