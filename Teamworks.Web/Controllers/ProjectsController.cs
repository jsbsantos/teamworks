using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using Teamworks.Core.Entities;

namespace Teamworks.Web.Controllers
{
    public class ProjectsController : ApiController
    {
        internal static readonly List<Project> Projects = new List<Project>()
                                                              {
                                                                  new Project()
                                                                      {
                                                                          Name = "Teamworks",
                                                                          Description = "Sample project"
                                                                      },
                                                                      new Project()
                                                                          {
                                                                              Name = "Codegarten",
                                                                              Description = "Failed project"
                                                                          }
                                                              };

        public IQueryable<Project> Get()
        {
            return Projects.AsQueryable();
        }

        public Project Get(int id)
        {
            if (Projects.Count < id)
            {
                return Projects[id];
            }
            throw new HttpResponseException(HttpStatusCode.NotFound);
        }
    }
}
