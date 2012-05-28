using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace Teamworks.Web.Controllers.Api.Handlers
{
    public class UnauthorizedHandler : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return base.SendAsync(request, cancellationToken).ContinueWith(
                t =>
                {
                    if (t.Result.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        t.Result.Headers.WwwAuthenticate.Add(
                            new AuthenticationHeaderValue("Basic", "realm=\"Api Teamworks\""));
                    }
                    return t.Result;
                });
        }
    }
}
