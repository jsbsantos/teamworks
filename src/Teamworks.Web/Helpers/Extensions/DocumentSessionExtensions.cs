using System.Collections.Generic;
using Raven.Bundles.Authorization.Model;
using Raven.Client;
using Raven.Client.Authorization;
using Teamworks.Core.People;

namespace Teamworks.Web.Helpers.Extensions
{
    public static class DocumentSessionExtensions
    {
        public static void SetAuthorizationForUser(this IDocumentSession session, object entity, Person person)
        {
            var doc = session.GetAuthorizationFor(entity);
            var list = doc.Permissions ?? (doc.Permissions = new List<DocumentPermission>());
            list.Add(new DocumentPermission()
                         {
                             Allow = true,
                             Operation = "Operation",
                             User = person.Id
                         });
            session.SetAuthorizationFor(entity, doc);
        }
    }
}