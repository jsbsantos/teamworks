using System;
using System.Linq;
using System.Security.Principal;
using System.Threading;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Raven.Client;
using Teamworks.Core;
using Teamworks.Core.Authentication;

namespace Teamworks.Web.Attributes.Api
{
    public class FormsAuthenticationFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext context)
        {
            IIdentity identity = HttpContext.Current.User.Identity;
            if (!string.IsNullOrEmpty(identity.Name) &&
                identity.AuthenticationType.Equals("Forms", StringComparison.OrdinalIgnoreCase))
            {
                var session = context.Request.Properties[Application.Keys.RavenDbSessionKey] as IDocumentSession;
                var person = session.Load<Person>(identity.Name);
                if (person != null)
                {
                    identity = new PersonIdentity(person);
                    Thread.CurrentPrincipal = new GenericPrincipal(identity, person.Roles.ToArray());
                }
            }

            base.OnActionExecuting(context);
        }
    }
}