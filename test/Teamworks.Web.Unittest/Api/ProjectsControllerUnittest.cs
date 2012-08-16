using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Routing;
using Raven.Client;
using Teamworks.Core;
using Teamworks.Core.Services;
using Teamworks.Web.Controllers.Api;
using Teamworks.Web.Unittest.Api.Fixture;
using Teamworks.Web.ViewModels.Api;
using Xunit;

namespace Teamworks.Web.Unittest.Api
{
    /*
    public class ProjectsControllerUnittest : BaseControllerUnittest
    {
        protected override string Url
        {
            get { return "http://localhost/api/projects"; }
        }

        protected override IHttpRouteData RouteData(HttpConfiguration config)
        {
            var route = config.Routes.MapHttpRoute("Projects_GetById",
                                                   "api/{controller}/{id}");
            return new HttpRouteData(route, new HttpRouteValueDictionary {{"controller", "projects"}});
        }

        [Fact]
        public void GetProjects()
        {
            var size = 0;
            using (var session = RavenDbFixture.DocumentStore.OpenSession())
            {
                size = session.Query<Project>().Count();
            }

            List<ProjectViewModel> result;
            using (var session = RavenDbFixture.DocumentStore.OpenSession())
            {
                var controller = ControllerForTests<ProjectsController>(session, HttpMethod.Get);
                result = controller.Get().ToList();
            }

            Assert.Equal(size, result.Count());
            Assert.Equal(0, result.Count(s => s == null));
        }

        [Fact]
        public void GetProjectById()
        {
            Project expected;
            int projectId = 4;
            Fixture.Populate(PopulateAProject);
            using (var session = RavenDbFixture.DocumentStore.OpenSession())
            {
                expected = session.Load<Project>(projectId);
            }

            ProjectViewModel result;
            using (var session = RavenDbFixture.DocumentStore.OpenSession())
            {
                var controller = ControllerForTests<ProjectsController>(session, HttpMethod.Get);
                result = controller.Get(projectId);
            }

            Assert.NotNull(result);
            Assert.Equal(expected.Name, result.Name);
            Assert.Equal(expected.Description, result.Description);
        }


        [Fact]
        public void PostProjectReturnsCreatedStatusCode()
        {
            HttpResponseMessage response;
            using (var session = RavenDbFixture.DocumentStore.OpenSession())
            {
                var controller = ControllerForTests<ProjectsController>(session, HttpMethod.Post);
                response = controller.Post(new ProjectViewModel
                                               {
                                                   Name = "post project",
                                                   Description = "description post project"
                                               });
                session.SaveChanges();
            }

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Fact]
        public void PostProjectIsPersistedInDb()
        {
            const string name = "post project";
            const string description = "description post project";

            HttpResponseMessage response;
            using (var session = RavenDbFixture.DocumentStore.OpenSession())
            {
                var controller = ControllerForTests<ProjectsController>(session, HttpMethod.Post);
                response = controller.Post(new ProjectViewModel
                                               {
                                                   Name = name,
                                                   Description = description
                                               });
                session.SaveChanges();
            }

            var result = response.Content.ReadAsAsync<ProjectViewModel>().Result;

            using (var session = RavenDbFixture.DocumentStore.OpenSession())
            {
                var project = session.Load<Project>(result.Id);

                Assert.NotNull(project);
                Assert.Equal(name, project.Name);
                Assert.Equal(description, project.Description);

                Assert.False(project.Archived);
            }
        }

        [Fact]
        public void PostProjectReturnsTheCorrectLocationInResponse()
        {
            HttpResponseMessage response;
            using (var session = RavenDbFixture.DocumentStore.OpenSession())
            {
                var controller = ControllerForTests<ProjectsController>(session, HttpMethod.Post);
                response = controller.Post(new ProjectViewModel
                                               {
                                                   Name = "post project",
                                                   Description = "description post project"
                                               });
                session.SaveChanges();
            }

            var resuilt = response.Content.ReadAsAsync<ProjectViewModel>().Result;
            Assert.Equal("http://localhost/api/projects/" + resuilt.Id, response.Headers.Location.ToString());
        }

        [Fact]
        public void DeleteProjectReturnsNoContentStatusCode()
        {

            const int projectId = 4;
            Fixture.Populate(PopulateAProject);

            HttpResponseMessage response;
            using (var session = RavenDbFixture.DocumentStore.OpenSession())
            {
                var controller = ControllerForTests<ProjectsController>(session, HttpMethod.Delete);
                response = controller.Delete(projectId);

                session.SaveChanges();
            }
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
         
        }

        [Fact]
        public void DeleteProjectPersistedInDb()
        {
            const int projectId = 4;
            Fixture.Populate(PopulateAProject);

            using (var session = RavenDbFixture.DocumentStore.OpenSession())
            {
                var controller = ControllerForTests<ProjectsController>(session, HttpMethod.Delete);
                controller.Delete(4);

                session.SaveChanges();
            }

            using (var session = RavenDbFixture.DocumentStore.OpenSession())
            {
                Assert.Null(session.Load<Project>(projectId));
            }
        }

        public void Populate(IDocumentSession session)
        {
            foreach (var p in Enumerable.Range(1, 3))
            {
                session.Store(new Project
                                  {
                                      Id = p.ToId("project"),
                                      Name = "proj " + p,
                                      Description = "description " + p
                                  });
            }
        }

        public void PopulateAProject(IDocumentSession session)
        {
            session.Store(new Project
                              {
                                  Id = 4.ToId("project"),
                                  Name = "proj 4",
                                  Description = "description 4"
                              });
        }
    }

     */
}