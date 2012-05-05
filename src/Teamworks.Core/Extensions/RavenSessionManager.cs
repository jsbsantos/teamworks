using Raven.Client;
using Raven.Client.Document;

namespace Teamworks.Core.Extensions {
    public static class RavenSessionManager {
        private static IDocumentStore store;

        public static IDocumentStore DocumentStore {
            get { return store ?? (store = new DocumentStore {ConnectionStringName = "RavenDB"}.Initialize()); }
        }

        public static IDocumentSession NewSession() {
            return DocumentStore.OpenSession();
        }
    }
}