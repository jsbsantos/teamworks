using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Hosting;
using Teamworks.Core.Authentication;
using Teamworks.Core.People;
using Teamworks.Web.Helpers.Extensions;

namespace Teamworks.Web.Helpers.Handlers {
    public class AuthHandler : DelegatingHandler {
        private const string AuthToken = "auth_token";

        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken) {
            var token = request.QueryString(AuthToken);
            if (token != null) {
                var person = Person.Forge("a@b.c", "fampinheiro", "1q2w3e4r5t");
                /* todo 
                var session = Global.Raven.CurrentSession.Load<Session>(token);
                return session == null ? Unauthorized() : base.SendAsync(request, cancellationToken);
                */
                var identity = new PersonIdentity(person);
                request.Properties[HttpPropertyKeys.UserPrincipalKey] =  new GenericPrincipal(identity, person.Roles.ToArray());
            }
            return base.SendAsync(request, cancellationToken);
        }
    }
}