using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;
using Raven.Client;
using Raven.Client.Document;

namespace Teamworks.Web.Controllers.Api {
    public static class RavenDocumentHolderAndSessionHandler {
        private static readonly ConcurrentDictionary<Type, Accessors> Cache =
            new ConcurrentDictionary<Type, Accessors>();

        public static string ConnectionString { get; set; }

        private static IDocumentStore _store;

        public static IDocumentStore DocumentStore {
            get { return _store ?? (_store = CreateDocumentStore()); }
        }

        public static IDocumentSession InjectSessionIfPropetyAvailable(object instance) {
            Type t = instance.GetType();
            Accessors accessors = Cache.GetOrAdd(t, Accessors.Create(t));
            if (accessors == null) {
                return null;
            }

            var session = DocumentStore.OpenSession();
            accessors.Set(instance, session);

            return session;
        }

        public static void SaveSessionIfAvailable(object instance) {
            Accessors accessors;
            if (Cache.TryGetValue(instance.GetType(), out accessors) && accessors != null) {
                using (var session = accessors.Get(instance)) {
                    if (session == null) {
                        return;
                    }
                    session.SaveChanges();
                }
            }
        }

        private static IDocumentStore CreateDocumentStore() {
            if (string.IsNullOrEmpty(ConnectionString)) {
                throw new InvalidOperationException("Connection string must be setted.");
            }
            return new DocumentStore
                   {
                       ConnectionStringName = ConnectionString
                   }.Initialize();
        }


        private class Accessors {
            public Action<object, IDocumentSession> Set;
            public Func<object, IDocumentSession> Get;

            public static Accessors Create(Type type) {
                const BindingFlags flags = BindingFlags.GetField | BindingFlags.SetField | BindingFlags.Instance;
                var prop = type.GetProperties(flags).FirstOrDefault(p => p.PropertyType == typeof (IDocumentSession));
                if (prop == null) {
                    return null;
                }

                return new Accessors
                       {
                           Set = (i, s) => prop.SetValue(i, s, null),
                           Get = i => (IDocumentSession) prop.GetValue(i, null)
                       };
            }
        }
    }
}