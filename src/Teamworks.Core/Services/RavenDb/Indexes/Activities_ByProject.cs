using System.Linq;
using Raven.Client.Indexes;

namespace Teamworks.Core.Services.RavenDb.Indexes
{
    public class Activities_ByProject : AbstractIndexCreationTask<Activity>
    {
        public Activities_ByProject()
        {
            Map = activities => from act in activities
                                select new
                                           {
                                               act.Project
                                           };
        }
    }
}