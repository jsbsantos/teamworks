using System;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Indexes;
using Teamworks.Core.Services.RavenDb.Indexes;
using Teamworks.Core.Services.Storage;

namespace Teamworks.Core.Services.RavenDb
{
    public class Session
    {
        private const string Key = "RAVEN_CURRENT_SESSION_KEY";

        private static readonly Lazy<Session> _instance =
            new Lazy<Session>(() => new Session());

        public readonly IDocumentStore Store;

        private Session()
        {
            Store = new DocumentStore
                        {
                            ConnectionStringName = "RavenDB"
                        }.RegisterListener(new PersonQueryListenter())
                        .Initialize();
        }

        public static Session Instance
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