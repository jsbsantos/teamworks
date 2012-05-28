using System;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Hosting;
using Teamworks.Core.Authentication;
using Teamworks.Core.People;
using Teamworks.Core.Services;

namespace Teamworks.Web.Controllers.Api.Handlers
{
    public class BasicAuthenticationHandler : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var authorization = request.Headers.Authorization;
            if (authorization.Scheme.Equals("Basic", StringComparison.OrdinalIgnoreCase))
            {
                var basic = Convert.FromBase64String(authorization.Parameter);
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