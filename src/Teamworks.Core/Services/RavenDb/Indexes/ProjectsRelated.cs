using System.Linq;
using Raven.Client.Indexes;

namespace Teamworks.Core.Services.RavenDb.Indexes
{
    public class ProjectsEntitiesRelated : AbstractMultiMapIndexCreationTask<ProjectsEntitiesRelated.Result>
    {
        public ProjectsEntitiesRelated()
        {
            AddMap<Activity>(activities => from a in activities
                                           select new
                                                      {
                                                          EntityId = a.Id,
                                                          a.Project,
                                                          a.Name
                                                      });
            AddMap<Discussion>(discussions => from d in discussions
                                              select new
                                                         {
                                                             EntityId = d.Id,
                                                             Project = d.Entity,
                                                             d.Name
                                                         });
        }

        public override string IndexName
        {
            get { return "Projects/EntitiesRelated"; }
        }

        #region Nested type: Result

        public class Result
        {
            public string EntityId { get; set; }
            public string Project { get; set; }
            public string Name { get; set; }
        }

        #endregion
    }
}