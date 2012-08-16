using System;
using System.Collections.Generic;
using System.Linq;
using Raven.Bundles.Authorization.Model;
using Teamworks.Core.Services;

namespace Teamworks.Core
{
    public class Project : Entity
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public bool Archived { get; set; }

        public IList<string> People { get; set; }

        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset CreatedAt { get; set; }

        public IList<OperationPermission> Permissions { get; set; }

        public static Project Forge(string name, string description, DateTimeOffset? startdate = null)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("name");
            }

            return new Project
                       {
                           Name = name,
                           Description = description ?? "",
                           People = new List<string>(),
                           Permissions = new List<OperationPermission>(),
                           StartDate = startdate ?? DateTimeOffset.UtcNow,
                           CreatedAt = DateTimeOffset.UtcNow
                       };
        }


        public void AllowPersonAssociation()
        {
            Permissions.Add(new OperationPermission
                                {
                                    Allow = true,
                                    Operation = Global.Constants.Operation,
                                    Tags = {Id}
                                });
        }
    }
}