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
                            ConnectionStringName = "RavenDB"
                        }.Initialize();
        }

        public static Raven Instance
        {
            get { return _instance.Value; }
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

                Local.Data[Key] = session = Store.OpenSession();
                return session;
            }
        }

        public void Reset()
        {
            Reset(false);
        }

        public void Reset(bool save)
        {
            if (save)
            {
                CurrentSession.SaveChanges();
            }
            Local.Data[Key] = null;
        }
    }
}