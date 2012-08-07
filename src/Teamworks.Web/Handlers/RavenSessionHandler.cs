using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Raven.Client;
using Teamworks.Core.Services;

namespace Teamworks.Web.Handlers
{
    public class RavenSessionHandler : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
            IDocumentSession session = Global.Database.OpenSession();
            request.Properties[Application.Keys.RavenDbSessionKey] = session;
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