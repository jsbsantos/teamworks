using System.Collections.Generic;
using System.Linq;
using Raven.Client.Listeners;
using Raven.Json.Linq;

namespace Teamworks.Core.Services.RavenDb
{
    public class DocumentStoreListener : IDocumentStoreListener
    {
        private Dictionary<string, dynamic> changes;

        public DocumentStoreListener()
        {
            changes = new Dictionary<string, dynamic>();
            ;
        }

        public bool BeforeStore(string key, object entityInstance, RavenJObject metadata, RavenJObject original)
        {
            if (entityInstance is Core.Discussion)
            {
                var instance = entityInstance as Core.Discussion;

                changes[key] = new
                                   {
                                       Messages =
                                           instance.Messages.Count > original.Value<RavenJArray>("Messages").Count()
                                   };
            }
            return true;
        }

        public void AfterStore(string key, object entityInstance, RavenJObject metadata)
        {
            var instance = entityInstance as Core.Discussion;

            if (instance != null)
            {
                if (changes.ContainsKey(key) && changes[key].Messages)
                {
                    Global.Executor.Enqueue(() =>
                                                {
                                                    using (var session = Global.Database.OpenSession())
                                                    {
                                                        var people =
                                                            session.Load<Core.Person>(instance.Subscribers).Select(
                                                                x => x.Email).ToList();
                                                        instance.Notify(instance.Messages.Last(), people);
                                                    }
                                                });
                    changes.Remove(key);
                }
            }
        }
    }
}

/*

 */