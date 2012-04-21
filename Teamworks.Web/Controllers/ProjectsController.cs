using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using System.Web.Http.ModelBinding.Binders;

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
                throw new HttpResponseException(HttpStatusCode.NotFound);
            return Projects[id];
        }

        public HttpResponseMessage<Models.Project> Post(Models.Project project)
        {
            project.Url = "/api/projects/" + Projects.Count;
            
            Projects.Add(project);
            var response = new HttpResponseMessage<Models.Project>(project, HttpStatusCode.Created);
            var uri = Request.RequestUri.Authority + project.Url;
            response.Headers.Location = new Uri(uri);
            return response;
        }

        public HttpResponseMessage Put([ModelBinder(typeof(TypeConverterModelBinder))]int id, Models.Project project)
        {
            if (Projects.Count < id)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            var p = Projects[id];
            p.Name = project.Name ?? p.Name;
            p.Description = project.Description ?? p.Description;
            return new HttpResponseMessage(HttpStatusCode.NoContent);
        } 

        internal static readonly List<Models.Project> Projects = new List<Models.Project>
                                                              {
                                                                  new Models.Project
                                                                      {
                                                                          Url = "/api/projects/0",
                                                                          Name = "Teamworks",
                                                                          Description = "Sample project"
                                                                      },
                                                                  new Models.Project
                                                                      {
                                                                          Url = "/api/projects/1",
                                                                          Name = "Codegarten",
                                                                          Description = "Failed project"
                                                                      }
                                                              };
    }
}