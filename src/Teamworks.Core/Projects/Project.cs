using System.Collections.Generic;
using Raven.Bundles.Authorization.Model;
using Teamworks.Core.Services;

namespace Teamworks.Core
{
    public class Project : Entity
    {
        public string Name { get; set; }
        public bool Archived { get; set; }
        public string Description { get; set; }

        public IList<string> People { get; set; }
        public IList<string> Activities { get; set; }
        public IList<string> Discussions { get; set; }

        public IList<OperationPermission> Permissions { get; set; }

        public static Project Forge(string name, string description)
        {
            return new Project
                       {
                           Name = name ?? "",
                           Description = description ?? "",
                           People = new List<string>(),
                           Activities = new List<string>(),
                           Discussions = new List<string>(),
                           Permissions = new List<OperationPermission>()
                       };
        }

        public void AllowPersonAssociation()
        {
            Permissions.Add(new OperationPermission()
                                {
                                    Allow = true,
                                    Operation = Global.Constants.Operation,
                                    Tags = {Id}
                                });

        }
    }
}