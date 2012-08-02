using Raven.Client.Embedded;
using Raven.Client.Util;
using Teamworks.Core.Services.RavenDb;

namespace Teamworks.Web.Test.Api.Fixture
{
    public class DocumentStoreFixture
    {
        public void Initialize()
        {
            Database.Store =
                new EmbeddableDocumentStore
                    {
                        ConnectionStringName = "RavenDB"
                    }.Initialize();
        }

        public void Store(object entity)
        {
            using (var session = Database.Store.OpenSession())
            {
                session.Store(entity);
                session.SaveChanges();
            }
        }

        public T Load<T>(string id)
        {
            using (var session = Database.Store.OpenSession())
            {
                return session.Load<T>(id);
            }
        }
    }
}