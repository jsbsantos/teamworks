using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Hosting;
using Teamworks.Core;
using Teamworks.Core.Authentication;
using Teamworks.Core.People;
using Teamworks.Web.Helpers.Extensions;

namespace Teamworks.Web.Helpers.Handlers
{
    public class AuthenticationHandler : DelegatingHandler
    {
        private const string AuthToken = "auth_token";

        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
            string token = request.QueryString(AuthToken);
            if (token != null)
            {
                var session = Global.Raven.CurrentSession
                    .Include<Token>(t => t.Person)
                    .Load<Token>("tokens/" + token);

                if (session != null)
                {
                    var person = Global.Raven.CurrentSession.Load<Person>(session.Person);
                    if (person != null)
                    {
                        //todo add NLog message
                        var identity = new PersonIdentity(person);
                        request.Properties[HttpPropertyKeys.UserPrincipalKey] =
                            new GenericPrincipal(identity, person.Roles.ToArray());
                    }
                }
            }
            return base.SendAsync(request, cancellationToken).ContinueWith(t =>
                                                                               {
                                                                                   if (t.Result.StatusCode ==
                                                                                       HttpStatusCode.Unauthorized)
                                                                                   {
                                                                                       t.Result.Headers.WwwAuthenticate.
                                                                                           Add(
                                                                                               new AuthenticationHeaderValue
                                                                                                   (
                                                                                                   "Basic",
                                                                                                   "realm=\"Api Teamworks\""));
                                                                                   }
                                                                                   return t.Result;
                                                                               });
        }
    }
}