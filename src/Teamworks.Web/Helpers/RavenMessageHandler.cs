using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Raven.Client;
using Teamworks.Core.Extensions;
using Global = Teamworks.Web.Models.Global;

namespace Teamworks.Web.Helpers {
    public class RavenMessageHandler : DelegatingHandler {
        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken) {
            IDocumentSession session = Global.DocumentStore.OpenSession();
            Local.Data[Global.RavenKey] = session;
            return base.SendAsync(request, cancellationToken)
                .ContinueWith(t => {
                                  using (session) {
                                      if (session != null && t.IsCompleted && !t.IsFaulted) {
                                          session.SaveChanges();
                                      }
                                  }
                                  return t.Result;
                              });
        }
    }
}