using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Teamworks.Core;

namespace Teamworks.Web.Helpers.Handlers
{
    public class RavenHandler : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var session = Global.Raven.CurrentSession;
            return base.SendAsync(request, cancellationToken)
                .ContinueWith(t =>
                                  {
                                      using (session)
                                      {
                                          if (session != null && t.IsCompleted && !t.IsFaulted)
                                          {
                                              session.SaveChanges();
                                          }
                                      }
                                      return t.Result;
                                  });
        }
    }
}