using System;
using Raven.Client;
using Raven.Client.Embedded;
using Raven.Client.Indexes;
using Raven.Client.Listeners;
using Teamworks.Core.Services.RavenDb.Indexes;
using Teamworks.Web.Helpers.AutoMapper;

namespace Teamworks.Web.Unittest.Api.Fixture
{
    public class RavenDbFixture
    {
        public static IDocumentStore DocumentStore;

        public RavenDbFixture()
        {
            if (DocumentStore != null) return;

            DocumentStore =
                new EmbeddableDocumentStore
                {
                    RunInMemory = true,
                }.RegisterListener(new NoStaleQueriesAllowed())
                .Initialize();

            AutoMapperConfiguration.Configure();
            IndexCreation.CreateIndexes(typeof(Activities_ByProject).Assembly, DocumentStore);
        }

        public class NoStaleQueriesAllowed : IDocumentQueryListener
        {
            public void BeforeQueryExecuted(IDocumentQueryCustomization queryCustomization)
            {
                queryCustomization.WaitForNonStaleResultsAsOfNow();
            }
        }

        public void Populate(Action<IDocumentSession> action)
        {
            using (var session = DocumentStore.OpenSession())
            {
                action(session);
                session.SaveChanges();
            }
        }
    }
}