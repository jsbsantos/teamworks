using System;
using System.Linq;
using Raven.Bundles.Authorization.Model;
using Raven.Client;
using Raven.Client.Authorization;
using Teamworks.Core.Services;

namespace Teamworks.Core.Extensions
{
    public static class ProjectExtensions
    {
        public static void Grant(this Project project, string operation, Person person)
        {
            var op = Global.Constants.Projects;
            if (!string.IsNullOrWhiteSpace(operation))
                op += "/" + operation;

            person.Permissions.Add(new OperationPermission
                                       {
                                           Allow = true,
                                           Operation = op,
                                           Tags = { project.Id }
                                       });
            project.People.Add(person.Id);
        }

        public static void Revoke(this Project project, string operation, Person person)
        {
            var op = Global.Constants.Projects;
            if (!string.IsNullOrWhiteSpace(operation))
                op += "/" + operation;
            
            var permissions = person.Permissions;
            permissions.Remove(permissions.FirstOrDefault(p => p.Operation == op));

            if (!permissions.Any(p => p.Operation.StartsWith(project.Id)))
                project.People.Remove(person.Id);
        }

        public static void Initialize(this Entity entity, IDocumentSession session)
        {
            if (string.IsNullOrWhiteSpace(entity.Id))
                throw new NullReferenceException("project.Id");
            
            var permission = new DocumentPermission
                                 {
                                     Allow = true,
                                     Operation =
                                         Global.Constants.Projects,
                                 };
            var authorization = new DocumentAuthorization
                                    {
                                        Permissions = {permission},
                                        Tags = { entity.Id }
                                    };
            session.SetAuthorizationFor(entity, authorization);
        }
    }
}