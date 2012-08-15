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

        public DateTime StartDate { get; set; }
        public DateTime CreatedAt { get; set; }

        public IList<OperationPermission> Permissions { get; set; }

        public static Project Forge(string name, string description, DateTime? startdate = null)
        {
            return new Project
                       {
                           Name = name ?? "",
                           Description = description ?? "",
                           People = new List<string>(),
                           Permissions = new List<OperationPermission>(),
                           StartDate = startdate ?? DateTime.Now,
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