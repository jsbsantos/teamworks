using System;
using System.Collections.Generic;
using System.Linq;
using Teamworks.Core.Services;
using Teamworks.Web.Models.Api;
using Activity = Teamworks.Core.Activity;
using Project = Teamworks.Core.Project;

namespace Teamworks.Web.Helpers.Teamworks
{
    public static class GraphExtensions
    {
        public static List<ActivityRelation> DependencyGraph(this Project project)
        {
            var activities = Global.Database.CurrentSession.Load<Activity>(project.Activities);
            var relation = new List<ActivityRelation>();

            foreach (var activity in activities)
                relation.AddRange(activity.DependencyGraph());

            return relation.ToList();
        }

        public static List<ActivityRelation> DependencyGraph(this Activity activity)
        {
            var parents = Global.Database.CurrentSession.Load<Activity>(activity.Dependencies).ToList();
            return activity.Dependencies.Select(p =>
                                                    {
                                                        var parent = parents.Single(x => p.Equals(x.Id,
                                                                                                  StringComparison.
                                                                                                      InvariantCultureIgnoreCase));
                                                        return new ActivityRelation
                                                                   {
                                                                       Parent = parent.Identifier,
                                                                       Activity = activity.Identifier,
                                                                       Duration = parent.Duration,
                                                                   };
                                                    }).ToList();
        }
    }
}