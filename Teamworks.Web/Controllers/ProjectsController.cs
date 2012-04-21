using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Teamworks.Web.Controllers
{
    public class ProjectsController : ApiController
    {
        public IQueryable<Models.Project> Get()
        {
            return Projects.AsQueryable();
        }

        public Models.Project Get(int id)
        {
            if (Projects.Count < id)
            {
                return Projects[id];
            }
            throw new HttpResponseException(HttpStatusCode.NotFound);
        }

        public HttpResponseMessage<Models.Project> Post(Models.Project project)
        {
            Projects.Add(project);
            var response = new HttpResponseMessage<Models.Project>(project, HttpStatusCode.Created);
            var uri = Request.RequestUri.AbsoluteUri + "/" + Projects.Count;
            response.Headers.Location = new Uri(uri);
            return response;
        }

        internal static readonly List<Models.Project> Projects = new List<Models.Project>
                                                              {
                                                                  new Models.Project
                                                                      {
                                                                          Name = "Teamworks",
                                                                          Description = "Sample project"
                                                                      },
                                                                  new Models.Project
                                                                      {
                                                                          Name = "Codegarten",
                                                                          Description = "Failed project"
                                                                      }
                                                              };
    }
}