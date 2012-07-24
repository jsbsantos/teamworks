using System;
using System.Collections.Generic;
using System.Linq;
using Teamworks.Core;
using Teamworks.Core.Services;

namespace Teamworks.Web.Helpers.Teamworks
{
    public static class GraphExtensions
    {
        public static List<int[]> DependencyGraph(this Project project)
        {
            var activities = Global.Database.CurrentSession.Load<Activity>(project.Activities);
            var relation = new List<int[]>();

            foreach (var activity in activities)
                relation.AddRange(activity.DependencyGraph());
            
            return relation.Where(i => !relation.Contains(new[] {i[1], i[0]})).ToList();
        }

        public static List<int[]> DependencyGraph(this Activity activity)
        {
            return activity.Dependencies.Select(p => new[] {activity.Identifier, StripIdentityName(p)}).ToList();
        }

        private static int StripIdentityName(string Id)
        {
            int i;
            if (string.IsNullOrEmpty(Id) || (i = Id.IndexOf('/')) < 0)
            {
                return 0;
            }

            int id;
            return int.TryParse(Id.Substring(i + 1, Id.Length - i - 1), out id) ? id : 0;
        }
    }
}