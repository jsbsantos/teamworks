using Raven.Client.Embedded;
using Teamworks.Core.Services.RavenDb;

namespace Teamworks.Web.Test.Api
{
    public class EmbeddableDocumentStoreFixture
    {
        public void Initialize()
        {
            Session.Store =
                new EmbeddableDocumentStore
                    {
                        UseEmbeddedHttpServer = true
                    }.RegisterListener(new PersonQueryListenter())
                    .Initialize();
        }
    }
}