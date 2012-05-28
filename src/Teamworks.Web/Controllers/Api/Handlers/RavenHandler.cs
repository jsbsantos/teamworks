using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Raven.Client;
using Teamworks.Core.Services;

namespace Teamworks.Web.Controllers.Api.Handlers
{
    public class RavenHandler : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var session = Global.Raven.Open();
            return base.SendAsync(request, cancellationToken)
                .ContinueWith(t =>
                                  {
                                      using (session)
                                      {
                                          if (session != null && t.Result.IsSuccessStatusCode)
                                          {
                                              session.SaveChanges();
                                          }
                                      }
                                      return t.Result;
                                  });
        }
    }
}