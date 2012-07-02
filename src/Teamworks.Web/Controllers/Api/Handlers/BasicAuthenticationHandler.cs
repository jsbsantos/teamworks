using System;
using System.Linq;
using System.Net.Http;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Hosting;
using Teamworks.Core;
using Teamworks.Core.Authentication;
using Teamworks.Core.Services;

namespace Teamworks.Web.Controllers.Api.Handlers
{
    public class BasicAuthenticationHandler : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var header = request.Headers.Authorization;
            if (header != null && header.Scheme.Equals("Basic", StringComparison.OrdinalIgnoreCase))
            {
                var basic = Convert.FromBase64String(header.Parameter);
                var credentials = Encoding.UTF8.GetString(basic).Split(':');
                if (credentials.Length == 2)
                {
                    Person person;
                    if (Global.Authentication.Basic(credentials[0], credentials[1], out person))
                    {
                        var identity = new PersonIdentity(person);
                        request.Properties[HttpPropertyKeys.UserPrincipalKey] =
                            new GenericPrincipal(identity, person.Roles.ToArray());                        
                    }
                }
            }
            return base.SendAsync(request, cancellationToken);
        }
    }
}