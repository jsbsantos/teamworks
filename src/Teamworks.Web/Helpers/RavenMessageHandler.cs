using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Teamworks.Core.Extensions;
using Global = Teamworks.Web.Models.Global;

namespace Teamworks.Web.Helpers {
    public class RavenMessageHandler : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var session = Global.DocumentStore.OpenSession();
            Local.Data[Global.RavenKey] = session;
            return base.SendAsync(request, cancellationToken)
                .ContinueWith(t =>
                              {
                                  using (session)
                                  {
                                      if (t.IsCompleted && session != null)
                                      {
                                          session.SaveChanges();
                                      }
                                  }
                                  return t.Result;
                              });
        }
    }
}