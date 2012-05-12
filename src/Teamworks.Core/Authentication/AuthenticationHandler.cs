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
using Teamworks.Core.Extensions;
using Teamworks.Core.People;

namespace Teamworks.Core.Authentication {
    //Web Api Authentication Handler
    public class AuthenticationHandler : DelegatingHandler {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
                                                               CancellationToken cancellationToken) {
            try {
                TryAuthenticateClient(request);
            }
            catch (SecurityTokenValidationException) {
                return Task<HttpResponseMessage>.Factory.StartNew(() => {
                                                                      var response =
                                                                          new HttpResponseMessage(
                                                                              HttpStatusCode.Unauthorized);
                                                                      SetAuthenticateHeader(response);

                                                                      return response;
                                                                  });
            }

            return base.SendAsync(request, cancellationToken).ContinueWith(
                (task) => {
                    HttpResponseMessage response = task.Result;

                    if (response.StatusCode == HttpStatusCode.Unauthorized) {
                        SetAuthenticateHeader(response);
                    }

                    return response;
                });
        }

        private void SetPrincipal(IPrincipal principal, HttpRequestMessage request) {
            Thread.CurrentPrincipal = principal;
            request.Properties[HttpPropertyKeys.UserPrincipalKey] = principal;
        }

        private void SetAuthenticateHeader(HttpResponseMessage response) {
            response.Headers.WwwAuthenticate.Add(
                new AuthenticationHeaderValue(AuthenticationManager.DefaultAuthenticationScheme)); //TODO EDIT
        }

        private void TryAuthenticateClient(HttpRequestMessage request) {
            AuthenticationHeaderValue header = request.Headers.Authorization;
            if (header != null) {
                NetworkCredential credentials = AuthenticationManager.GetCredentials(header.Scheme, header.Parameter);
                if (AuthenticationManager.Validate(header.Scheme, credentials)) {
                    var user =
                        Global.Raven.CurrentSession.Query<Person>().FirstOrDefault(
                            x => x.Id.Equals(credentials.UserName, StringComparison.InvariantCultureIgnoreCase));
                    if (user != null) {
                        var principal = new GenericPrincipal(new PersonIdentity(user), null);
                        SetPrincipal(principal, request);
                    }
                }
            }

            //todo allow anonymous?
            throw new SecurityTokenValidationException();
        }
    }
}