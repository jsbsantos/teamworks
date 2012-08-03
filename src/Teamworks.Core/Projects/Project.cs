using System;
using System.Collections.Generic;
using System.Linq;
using Raven.Imports.Newtonsoft.Json;
using Teamworks.Core.Services;

namespace Teamworks.Core
{
    public class Project : Entity
    {
        protected Project()
        {
            People = new List<string>();
        }

        public bool Archived { get; set; }
        public string Description { get; set; }
        
        [JsonIgnore]
        public IList<string> People { get; set; }
        public IList<string> Activities { get; set; }
        public IList<string> Discussions { get; set; }
        public DateTime StartDate { get; set; }
        
        public static Project Forge(string name, string description, DateTime? startdate)
        {
            return new Project
                       {
                           Name = name,
                           Description = description,
                           Activities = new List<string>(),
                           Discussions = new List<string>(),
                           People = new List<string>(),
                           StartDate = startdate ?? DateTime.Now,
                       };
        }


        public IEnumerable<ActivityRelation> DependencyGraph()
        {
            var activities = Global.Database.CurrentSession.Load<Activity>(Activities);
            var relation = new List<ActivityRelation>();

            foreach (var activity in activities)
                relation.AddRange(activity.DependencyGraph());

            return relation.ToList();
        }
    }
}