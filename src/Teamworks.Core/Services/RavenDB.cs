using System;
using Raven.Client;
using Raven.Client.Document;
using Teamworks.Core.Services.Storage;

namespace Teamworks.Core.Services
{
    public class RavenDB
    {
        private const string Key = "RAVEN_CURRENT_SESSION_KEY";

        private static readonly Lazy<RavenDB> _instance =
            new Lazy<RavenDB>(() => new RavenDB());

        public readonly IDocumentStore Store;

        private RavenDB()
        {
            Store = new DocumentStore
                        {
                            ConnectionStringName = "RavenDB"
                        }.Initialize();
        }

        public static RavenDB Instance
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