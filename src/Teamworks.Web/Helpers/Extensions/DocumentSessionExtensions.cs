using System.Collections.Generic;
using Raven.Bundles.Authorization.Model;
using Raven.Client;
using Raven.Client.Authorization;
using Teamworks.Core.People;

namespace Teamworks.Web.Helpers.Extensions
{
    public static class DocumentSessionExtensions
    {
        public static void SetAuthorizationForPerson(this IDocumentSession session, object entity, Person person)
        {
            var doc = session.GetAuthorizationFor(entity) ??
                      new DocumentAuthorization()
                          {
                              Permissions = new List<DocumentPermission>(),
                              Tags = new List<string>()
                          };

            doc.Permissions.Add(
                new DocumentPermission()
                    {
                        Allow = true,
                        Operation = "Operation",
                        User = person.Id
                    });
            session.SetAuthorizationFor(entity, doc);
        }
    }
}