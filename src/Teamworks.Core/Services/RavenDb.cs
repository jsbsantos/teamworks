using System;
using Raven.Client;
using Raven.Client.Document;
using Teamworks.Core.Services.Storage;

namespace Teamworks.Core.Services
{
    public class RavenDb
    {
        private const string Key = "RAVEN_CURRENT_SESSION_KEY";

        private static readonly Lazy<RavenDb> _instance =
            new Lazy<RavenDb>(() => new RavenDb());

        public readonly IDocumentStore Store;

        private RavenDb()
        {
            Store = new DocumentStore
                        {
                            ConnectionStringName = "RavenDB"
                        }.Initialize();
        }

        public static RavenDb Instance
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