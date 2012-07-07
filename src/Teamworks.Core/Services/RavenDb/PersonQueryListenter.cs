using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Raven.Client.Authorization;
using Raven.Json.Linq;
using Raven.Client.Listeners;

namespace Teamworks.Core.Services.RavenDb
{
    public class PersonQueryListenter : IDocumentConversionListener
    {
        public void EntityToDocument(object entity, RavenJObject document, RavenJObject metadata)
        {
            
        }

        public void DocumentToEntity(object entity, RavenJObject document, RavenJObject metadata)
        {
            var people = entity.GetType().GetProperty("People", typeof(IList<string>));
            if (people == null) return;
            
            var list = people.GetValue(entity, null) as IList<string>;
            if (list == null) return;
            
            var authorization = Global.Database.CurrentSession.GetAuthorizationFor(entity);
            if (authorization == null) return;

            foreach (var p in authorization.Permissions)
            {
                list.Add(p.User);
            }
        }
    }
}
