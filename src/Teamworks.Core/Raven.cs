using System;
using Raven.Client;
using Raven.Client.Document;
using Teamworks.Core.Extensions;

namespace Teamworks.Core {
    public class Raven {
        private static readonly Lazy<Raven> _instance =
            new Lazy<Raven>(() => new Raven());

        internal const string Key = "RAVEN_CURRENT_SESSION_KEY";
        public readonly IDocumentStore Store;

        private Raven() {
            Store = new DocumentStore
                    {
                        ConnectionStringName = "Raven"
                    }.Initialize();
        }

        public static Raven Instance {
            get { return _instance.Value; }
        }

        public IDocumentSession Open {
            get { return Store.OpenSession(); }
        }

        public IDocumentSession CurrentSession {
            get {
                var session = Local.Data[Key] as IDocumentSession;
                if (session != null) {
                    return session;
                }

                Local.Data[Key] = session = Open;
                return session;
            }
        }

        public void TryOpen() {
            if (Raven.Instance.CurrentSession != null)
            {
                return;
            }
            Local.Data[Raven.Key] = Raven.Instance.Store.OpenSession();
        }
    }
}