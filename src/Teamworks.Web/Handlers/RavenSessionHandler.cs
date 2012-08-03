using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Teamworks.Core.Services;
using Teamworks.Web.Helpers.Api;

namespace Teamworks.Web.Handlers
{
    public class RavenSessionHandler : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var session = request.GetOrOpenSession();
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