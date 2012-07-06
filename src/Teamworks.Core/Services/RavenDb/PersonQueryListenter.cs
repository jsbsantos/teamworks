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
            if (entity is Project)
            {
                var project = entity as Project;
                var authorization = Global.Database.CurrentSession.GetAuthorizationFor(entity);
                foreach (var p in authorization.Permissions)
                {
                    project.People.Add(p.User);
                }
            }
        }
    }
}
