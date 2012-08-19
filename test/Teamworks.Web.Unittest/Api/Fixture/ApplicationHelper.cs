using System;
using Raven.Client;
using Raven.Client.Embedded;
using Raven.Client.Indexes;
using Raven.Client.Listeners;
using Teamworks.Core.Services.RavenDb.Indexes;
using Teamworks.Web.Helpers.AutoMapper;

namespace Teamworks.Web.Unittest.Api.Fixture
{
    public class ApplicationHelper
    {
        public IDocumentStore OpenStore()
        {
            var store = new EmbeddableDocumentStore
            {
                RunInMemory = true
            }.RegisterListener(new NoStaleQueriesAllowed());
            store.Initialize();
            IndexCreation.CreateIndexes(typeof(Activities_ByProject).Assembly, store);
            return store;

        }

        public ApplicationHelper()
        {
            AutoMapperConfiguration.Configure();

        }

        public void Populate(IDocumentStore store, Action<IDocumentSession> action)
        {
            using (var session = store.OpenSession())
            {
                action(session);
                session.SaveChanges();
            }
        }

        #region Nested type: NoStaleQueriesAllowed

        public class NoStaleQueriesAllowed : IDocumentQueryListener
        {
            #region IDocumentQueryListener Members

            public void BeforeQueryExecuted(IDocumentQueryCustomization queryCustomization)
            {
                queryCustomization.WaitForNonStaleResultsAsOfNow();
            }

            #endregion
        }

        #endregion

    }
}