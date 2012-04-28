using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using System.Web.Http.ModelBinding.Binders;
using Teamworks.Web.Models;

namespace Teamworks.Web.Controllers
{
    public class ProjectsController : ApiController
    {
        public IQueryable<Project> Get()
        {
            return Projects.Values.AsQueryable();
        }

        public Project Get(int id)
        {
            var p = Projects[id];
            if (p == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            return p;
        }

        public HttpResponseMessage<Project> Post(Project project)
        {
            int id = Id++;
            project.Url = "/api/projects/" + id;

            Projects.Add(id, project);
            var response = new HttpResponseMessage<Project>(project, HttpStatusCode.Created);
            var uri = Request.RequestUri.Authority + project.Url;
            response.Headers.Location = new Uri(uri);
            return response;
        }

        /// <see cref="http://forums.asp.net/post/4855634.aspx"/>
        public HttpResponseMessage Put([ModelBinder(typeof (TypeConverterModelBinder))] int id, Models.Project project)
        {
            var p = Projects[id];
            if (p == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            p.Name = project.Name ?? p.Name;
            p.Description = project.Description ?? p.Description;
            return new HttpResponseMessage(HttpStatusCode.NoContent);
        }

        public HttpResponseMessage Delete(int id)
        {
            var p = Projects[id];
            if (p == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            Projects.Remove(id);
            return new HttpResponseMessage(HttpStatusCode.NoContent);
        }

        #region Dummy Data
        internal static int Id = 3;

        internal static readonly Dictionary<int, Project> Projects = new Dictionary<int, Project>()
                                                                         {
                                                                             {
                                                                                 1, new Project
                                                                                        {
                                                                                            Url = "/api/projects/1",
                                                                                            Name = "Teamworks",
                                                                                            Description =
                                                                                                "Sample project"
                                                                                        }
                                                                                 },
                                                                             {
                                                                                 2, new Project
                                                                                        {
                                                                                            Url = "/api/projects/2",
                                                                                            Name = "Codegarten",
                                                                                            Description =
                                                                                                "Failed project"
                                                                                        }
                                                                                 }
                                                                         };
        #endregion
    }
}