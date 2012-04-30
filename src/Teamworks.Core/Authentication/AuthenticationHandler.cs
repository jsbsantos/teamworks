using System;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Hosting;
using Raven.Client.Linq;
using Teamworks.Core.People;

namespace Teamworks.Core.Authentication
{
    //Web Api Authentication Handler
    public class AuthenticationHandler : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            try
            {
                TryAuthenticateClient(request);
            }
            catch (SecurityTokenValidationException)
            {
                return Task<HttpResponseMessage>.Factory.StartNew(() =>
                                                                      {
                                                                          var response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
                                                                          SetAuthenticateHeader(response);

                                                                          return response;
                                                                      });
            }

            return base.SendAsync(request, cancellationToken).ContinueWith(
                (task) =>
                {
                    var response = task.Result;

                    if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        SetAuthenticateHeader(response);
                    }

                    return response;
                });
        }

        private void SetPrincipal(IPrincipal principal, HttpRequestMessage request)
        {
            Thread.CurrentPrincipal = principal;
            request.Properties[HttpPropertyKeys.UserPrincipalKey] = principal;
        }

        private void SetAuthenticateHeader(HttpResponseMessage response)
        {
            response.Headers.WwwAuthenticate.Add(new AuthenticationHeaderValue(AuthenticationManager.DefaultAuthenticationScheme)); //TODO EDIT
        }

        private void TryAuthenticateClient(HttpRequestMessage request)
        {
            var header = request.Headers.Authorization;
            if (header != null)
            { 
                var credentials = AuthenticationManager.GetCredentials(header.Scheme, header.Parameter);
                if (AuthenticationManager.Validate(header.Scheme, credentials))
                {
                    var user = Person.Query().Where(x => x.Username.Equals(credentials.UserName, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
                    var principal = new GenericPrincipal(new GenericIdentity(user.Username), null);
                    SetPrincipal(principal, request);
                }
            }

            //todo allow anonymous?
            throw new SecurityTokenValidationException();
        }
    }
}