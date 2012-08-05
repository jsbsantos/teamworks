using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        public DateTime StartDate { get; set; }

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
            Permissions.Add(new OperationPermission()
                                {
                                    Allow = true,
                                    Operation = Global.Constants.Operation,
                                    Tags = {Id}
                                });
        }

        public IEnumerable<ActivityRelation> DependencyGraph(List<Activity> activities)
        {
            var relation = new List<ActivityRelation>();

            foreach (var activity in activities)
                relation.AddRange(activity.DependencyGraph(activities));

            return relation.ToList();
        }
    }
}