using System;
using System.Linq;
using System.Net.Http;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Hosting;
using Teamworks.Core;
using Teamworks.Core.Authentication;
using Teamworks.Core.Services;

namespace Teamworks.Web.Controllers.Api.Handlers
{
    public class FormsAuthenticationHandler : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var identity = HttpContext.Current.User.Identity;
            if (!string.IsNullOrEmpty(identity.Name) &&
                identity.AuthenticationType.Equals("Forms", StringComparison.OrdinalIgnoreCase))
            {
                var person = Global.Database.CurrentSession.Load<Person>(identity.Name);
                if (person != null)
                {
                    identity = new PersonIdentity(person);
                    Thread.CurrentPrincipal = new GenericPrincipal(identity, person.Roles.ToArray());                        
                }
            }
            return base.SendAsync(request, cancellationToken);
        }
    }
}