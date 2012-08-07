using System.Collections.Generic;
using System.Linq;
using Raven.Client.Indexes;

namespace Teamworks.Core.Services.RavenDb.Indexes
{
    public class ProjectEntityCount : AbstractMultiMapIndexCreationTask<ProjectEntityCount.Result>
    {
        public ProjectEntityCount()
        {
            AddMap<Project>(projects => from project in projects
                                        select new
                                                   {
                                                       Project = project.Id,
                                                       project.Name,
                                                       project.People,
                                                       NumberOfActivities = 0,
                                                       NumberOfDiscussions = 0
                                                   });
            AddMap<Activity>(activities => from activity in activities
                                           select new
                                                      {
                                                          Name = (string) null,
                                                          People = new List<string>(),
                                                          activity.Project,
                                                          NumberOfActivities = 1,
                                                          NumberOfDiscussions = 0
                                                      });
            AddMap<Discussion>(discussions => from discussion in discussions
                                              where discussion.Entity.StartsWith("project")
                                              select new
                                                         {
                                                             Name = (string) null,
                                                             People = new List<string>(),
                                                             Project = discussion.Entity,
                                                             NumberOfActivities = 0,
                                                             NumberOfDiscussions = 1
                                                         });
            Reduce = results => from result in results
                                group result by result.Project
                                into grouping
                                select new
                                           {
                                               Name = grouping.Select(s => s.Name).FirstOrDefault(s => s != null),
                                               People = grouping.Select(s => s.People).FirstOrDefault(s => s.Count > 0),
                                               Project = grouping.Key,
                                               NumberOfActivities = grouping.Sum(s => s.NumberOfActivities),
                                               NumberOfDiscussions = grouping.Sum(s => s.NumberOfDiscussions)
                                           };
        }

        public override string IndexName
        {
            get { return "Project/EntityCount"; }
        }

        #region Nested type: Result

        public class Result
        {
            public string Name { get; set; }
            public IList<string> People { get; set; }
            public string Project { get; set; }
            public int NumberOfActivities { get; set; }
            public int NumberOfDiscussions { get; set; }
        }

        #endregion
    }
}