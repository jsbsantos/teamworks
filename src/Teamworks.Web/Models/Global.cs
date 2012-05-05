using Raven.Client;
using Raven.Client.Document;

namespace Teamworks.Web.Models {
    public static class Global {
        private static IDocumentStore _store;

        public static string RavenKey {
            get { return "RAVEN_CURRENT_SESSION_KEY"; }
        }

        public static IDocumentStore DocumentStore {
            get { return _store ?? (_store = Initialize()); }
        }

        private static IDocumentStore Initialize() {
            return new DocumentStore
                   {
                       ConnectionStringName = "Raven"
                   }.Initialize();
        }
    }
}