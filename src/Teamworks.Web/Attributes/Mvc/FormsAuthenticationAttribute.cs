using System.Linq;
using System.Security.Principal;
using System.Web.Mvc;
using Raven.Client;
using Teamworks.Core;
using Teamworks.Core.Authentication;
using Teamworks.Web.Helpers.Mvc;

namespace Teamworks.Web.Attributes.Mvc
{
    public class FormsAuthenticationAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            IPrincipal user = context.HttpContext.User;
            if (user.Identity.IsAuthenticated)
            {
                string id = user.Identity.Name;
                if (!string.IsNullOrEmpty(id))
                {
                    IDocumentSession session = context.HttpContext.RavenSession();
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