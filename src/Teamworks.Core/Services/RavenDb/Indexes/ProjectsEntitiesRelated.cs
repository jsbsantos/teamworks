using System.Linq;
using Raven.Client.Indexes;

namespace Teamworks.Core.Services.RavenDb.Indexes
{
    public class ProjectsEntitiesRelated : AbstractMultiMapIndexCreationTask
    {
        public ProjectsEntitiesRelated()
        {
            AddMap<Activity>(activities => from a in activities
                                           select new 
                                                      {
                                                          Entity = a.Id,
                                                          Project = a.Project
                                                          
                                                      });
            AddMap<Discussion>(discussions => from d in discussions
                                              select new 
                                                         {
                                                             Entity = d.Id,
                                                             Project = d.Entity
                                                         });
        }

        public override string IndexName
        {
            get { return "Projects/EntitiesRelated"; }
        }
    }
}