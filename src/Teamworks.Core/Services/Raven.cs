using System;
using Raven.Client;
using Raven.Client.Document;

namespace Teamworks.Core.Services
{
    public class Raven
    {
        private const string Key = "RAVEN_CURRENT_SESSION_KEY";

        private static readonly Lazy<Raven> _instance =
            new Lazy<Raven>(() => new Raven());

        public readonly IDocumentStore Store;

        private Raven()
        {
            Store = new DocumentStore
                        {
                            ConnectionStringName = "RAVENHQ_CONNECTION_STRING"
                        }.Initialize();
        }

        public static Raven Instance
        {
            get { return _instance.Value; }
        }

        public IDocumentSession Open
        {
            get { return Store.OpenSession(); }
        }

        public IDocumentSession CurrentSession
        {
            get
            {
                var session = Local.Data[Key] as IDocumentSession;
                if (session != null)
                {
                    return session;
                }

                Local.Data[Key] = session = Open;
                return session;
            }
        }

        public void TryOpen()
        {
            if (Instance.CurrentSession != null)
            {
                return;
            }
            Local.Data[Key] = Instance.Store.OpenSession();
        }
    }
}