using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Raven.Client;
using Teamworks.Core;
using Teamworks.Core.Authentication;

namespace Teamworks.Web.Handlers
{
    public class BasicAuthentication : DelegatingHandler
    {
        public static Credentials GetBase64Credentials(string base64)
        {
            byte[] basic = Convert.FromBase64String(base64);
            string[] credentials = Encoding.UTF8.GetString(basic).Split(':');

            if (credentials.Length == 2)
            {
                return new Credentials
                           {
                               Username = credentials[0],
                               Password = credentials[1]
                           };
            }
            return new Credentials();
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
                                                               CancellationToken cancellationToken)
        {
            AuthenticationHeaderValue header = request.Headers.Authorization;
            if (header != null && header.Scheme.Equals("Basic", StringComparison.OrdinalIgnoreCase))
            {
                Credentials credentials = GetBase64Credentials(header.Parameter);
                var session = request.Properties[Application.Keys.RavenDbSessionKey] as IDocumentSession;
                Person person = session.Query<Person>().FirstOrDefault(
                    p => p.Username.Equals(credentials.Username, StringComparison.InvariantCultureIgnoreCase));

                if (person.IsThePassword(credentials.Password))
                {
                    var identity = new PersonIdentity(person);
                    Thread.CurrentPrincipal = new GenericPrincipal(identity, person.Roles.ToArray());
                }
            }

            return base.SendAsync(request, cancellationToken).ContinueWith(
                t =>
                    {
                        if (t.Result.StatusCode == HttpStatusCode.Unauthorized)
                        {
                            t.Result.Headers.WwwAuthenticate.Add(
                                new AuthenticationHeaderValue("Basic", "realm=\"Teamworks Api\""));
                        }
                        return t.Result;
                    });
            ;
        }

        #region Nested type: Credentials

        public class Credentials
        {
            public String Password;
            public string Username;
        }

        #endregion
    }
}