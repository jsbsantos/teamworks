using System.Collections.Generic;
using System.Linq;
using Raven.Bundles.Authorization.Model;
using Raven.Client.Authorization;
using Raven.Client.Listeners;
using Raven.Imports.Newtonsoft.Json;
using Raven.Json.Linq;

namespace Teamworks.Core.Services.RavenDb
{
    public class PersonConversionListener : IDocumentConversionListener
    {
        #region IDocumentConversionListener Members

        public void EntityToDocument(object entity, RavenJObject document, RavenJObject metadata)
        {
        }

        public void DocumentToEntity(object entity, RavenJObject document, RavenJObject metadata)
        {
            var property = entity.GetType().GetProperty("People", typeof (IList<string>));
            if (property == null || !property.GetCustomAttributes(typeof (JsonIgnoreAttribute), true).Any()) return;

            var list = property.GetValue(entity, null) as IList<string>;
            if (list != null) return;
            
            
            // GetAuthorizationFor copy
            var docAuthAsJson = metadata[AuthorizationClientExtensions.RavenDocumentAuthorization];
            if (docAuthAsJson == null) return;

            var authorization = new JsonSerializer {
                                                       ContractResolver = Global.Database.Conventions.JsonContractResolver,
                                                   }.Deserialize<DocumentAuthorization>(new RavenJTokenReader(docAuthAsJson));

            if (authorization == null) return;
            list = authorization.Permissions.Select(p => p.User).ToList();
            property.SetValue(entity, list, null);
        }

        #endregion
    }
}