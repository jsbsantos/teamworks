using System.Collections.Generic;
using System.Linq;
using Raven.Bundles.Authorization.Model;
using Raven.Client;
using Raven.Client.Authorization;
using Teamworks.Core;
using Teamworks.Core.Services;

namespace Teamworks.Web.Helpers.Extensions
{
    public static class DocumentSessionExtensions
    {
        public static Person GetPersonByEmail(this IDocumentSession session, string email)
        {
            return session.Query<Person>()
                .Where(p => p.Email == email).FirstOrDefault();
        }

        public static Person GetPersonByUsername(this IDocumentSession session, string username)
        {
            return session.Query<Person>()
                .Where(p => p.Username == username).FirstOrDefault();
        }

        public static Person GetPersonByLogin(this IDocumentSession session, string login)
        {
            return session.Query<Person>()
                .Where(p => p.Username == login || p.Email == login).FirstOrDefault();
        }

        public static void InitializePerson(this IDocumentSession session, Person person)
        {
            person.Permissions = new List<IPermission>
                                     {
                                         new OperationPermission() {
                                             Allow = true,
                                             Operation = Global.Constants.Operation,
                                             Tags = { person.Id }
                                         }
                                     };
        }

        public static void GrantAccessToProject(this IDocumentSession session, Project project, Person person)
        {
            var authorization = session.GetAuthorizationFor(project) ??
                                new DocumentAuthorization
                                    {
                                        Permissions = new List<DocumentPermission>(),
                                        Tags = new List<string>()
                                    };

            authorization.Permissions.Add(new DocumentPermission()
                                              {
                                                  Allow = true, 
                                                  Operation = Global.Constants.Operation,
                                                  User = person.Id,
                                                  Role = ""
                                              });

            session.SetAuthorizationFor(project, authorization);
        }

        public static void RevokeAccessToProject(this IDocumentSession session, Project project, Person person)
        {
            var authorization = session.GetAuthorizationFor(project) ??
                                new DocumentAuthorization
                                {
                                    Permissions = new List<DocumentPermission>(),
                                    Tags = new List<string>()
                                };

            var permission = authorization.Permissions.FirstOrDefault(p => p.User == person.Id);
            authorization.Permissions.Remove(permission);
        }
    }
}