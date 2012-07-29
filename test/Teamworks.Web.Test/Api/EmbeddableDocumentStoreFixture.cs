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
                        DataDirectory = "App_Data/Database"
                    }.RegisterListener(new PersonQueryListenter())
                    .Initialize();
        }
    }
}