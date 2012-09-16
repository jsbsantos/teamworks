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
        public static void Delete(this Project project, IDocumentSession session)
        {
            session.Query<Activity>()
                .Where(a => a.Project == project.Id).ToList().ForEach(a => a.Delete(session));
            session.Query<Discussion>()
                .Where(d => d.Entity == project.Id).ToList().ForEach(d => d.Delete(session));
            session.Delete(project);
        }

        public static void Grant(this Project project, string operation, Person person)
        {
            if (!string.IsNullOrWhiteSpace(operation))
                operation = Global.Constants.Projects;
            person.Permissions.Add(new OperationPermission
                {
                    Allow = true,
                    Operation = operation,
                    Tags = {project.Id}
                });
            project.People.Add(person.Id);
        }

        public static void Revoke(this Project project, string operation, Person person)
        {
            if (!string.IsNullOrWhiteSpace(operation))
                operation = Global.Constants.Projects;

            var permissions = person.Permissions;
            permissions.Remove(permissions.FirstOrDefault(p => p.Operation == operation));

            if (!permissions.Any(p => p.Operation.StartsWith(project.Id)))
            {
             if (project.People.Count == 1)
                 throw new InvalidOperationException("Project must always have at least one person.");

                project.People.Remove(person.Id);
            }
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
                    Tags = {entity.Id}
                };
            session.SetAuthorizationFor(entity, authorization);
        }
    }
}