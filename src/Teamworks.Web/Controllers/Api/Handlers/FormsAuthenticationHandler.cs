using System;
using System.Linq;
using System.Net.Http;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Hosting;
using Teamworks.Core.Authentication;
using Teamworks.Core.People;
using Teamworks.Core.Services;
using Teamworks.Web.Helpers.Extensions;

namespace Teamworks.Web.Controllers.Api.Handlers
{
    public class FormsAuthenticationHandler : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var id = HttpContext.Current.User.Identity.Name;
            if (!HttpContext.Current.User.Identity.AuthenticationType.Equals("Forms", StringComparison.OrdinalIgnoreCase)
                || string.IsNullOrEmpty(id))
            {
                return base.SendAsync(request, cancellationToken);
            }

            var person = Global.Raven.CurrentSession.Load<Person>(id);
            if (person != null)
            {
                var identity = new PersonIdentity(person);
                request.Properties[HttpPropertyKeys.UserPrincipalKey] =
                    new GenericPrincipal(identity, person.Roles.ToArray());
            }

            return base.SendAsync(request, cancellationToken);
        }
    }
}