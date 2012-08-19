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
using Teamworks.Web.ViewModels.Api;
using Xunit;

namespace Teamworks.Web.Unittest.Api
{
    public class ProjectsControllerTester : BaseControllerTester
    {
        protected override string Url
        {
            get { return "http://localhost/api/projects"; }
        }

        protected override IHttpRouteData RouteData(HttpConfiguration config)
        {
            var route = config.Routes.MapHttpRoute("api_projects_getbyid",
                                                   "api/{controller}/{id}");
            return new HttpRouteData(route, new HttpRouteValueDictionary { { "controller", "projects" } });
        }

        [Fact]
        public void GetProjects()
        {
            var store = Configure.OpenStore();
            Configure.Populate(store, Reset);
            
            const int expectedSize = 3;
            List<ProjectViewModel> result;
            using (var session = store.OpenSession())
            {
                var controller = ControllerForTests<ProjectsController>(session, HttpMethod.Get);
                result = controller.Get().ToList();
            }

            Assert.Equal(expectedSize, result.Count());
        }
        
        [Fact]
        public void GetProjectById()
        {
            var store = Configure.OpenStore();
            Configure.Populate(store, Reset);

            const int expectedProjectId = 1;
            const string expectedName = "proj 1";
            const string expectedDescription = "description 1";

            ProjectViewModel result;
            using (var session = store.OpenSession())
            {
                var controller = ControllerForTests<ProjectsController>(session, HttpMethod.Get);
                result = controller.GetById(expectedProjectId);
            }

            Assert.NotNull(result);
            Assert.Equal(expectedName, result.Name);
            Assert.Equal(expectedDescription, result.Description);
        }
        
        [Fact]
        public void PostProjectReturnsCreatedStatusCode()
        {
            var store = Configure.OpenStore();

            HttpResponseMessage response;
            using (var session = store.OpenSession())
            {
                session.Advanced.UseOptimisticConcurrency = true;
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
            var store = Configure.OpenStore();

            const string expectedName = "post project";
            const string expectedDescription = "description post project";
            
            HttpResponseMessage response;
            using (var session = store.OpenSession())
            {
                var controller = ControllerForTests<ProjectsController>(session, HttpMethod.Post);
                response = controller.Post(new ProjectViewModel
                                               {
                                                   Name = expectedName,
                                                   Description = expectedDescription
                                               });  
                session.SaveChanges();
            }

            var result = response.Content.ReadAsAsync<ProjectViewModel>().Result;
            using (var session = store.OpenSession())
            {
                var project = session.Load<Project>(result.Id);

                Assert.NotNull(project);
                Assert.Equal(expectedName, project.Name);
                Assert.Equal(expectedDescription, project.Description);

                Assert.False(project.Archived);
            }
        }

        [Fact]
        public void PostProjectReturnsTheCorrectLocationInResponse()
        {
            var store = Configure.OpenStore();

            HttpResponseMessage response;
            ProjectViewModel result;
            using (var session = store.OpenSession())
            {
                var controller = ControllerForTests<ProjectsController>(session, HttpMethod.Post);
                response = controller.Post(new ProjectViewModel
                                               {
                                                   Name = "post project",
                                                   Description = "description post project"
                                               });
                result = response.Content.ReadAsAsync<ProjectViewModel>().Result;
                session.SaveChanges();
            }

            Assert.Equal("http://localhost/api/projects/" + result.Id, response.Headers.Location.ToString());
        }

        [Fact]
        public void DeleteProjectReturnsNoContentStatusCode()
        {
            var store = Configure.OpenStore();
            Configure.Populate(store, Reset);

            const HttpStatusCode expectedStatusCode = HttpStatusCode.NoContent;
            
            HttpResponseMessage response;
            using (var session = store.OpenSession())
            {
                var controller = ControllerForTests<ProjectsController>(session, HttpMethod.Delete);
                response = controller.Delete(1);

                session.SaveChanges();
            }
            
            Assert.Equal(expectedStatusCode, response.StatusCode);
        }

        [Fact]
        public void DeleteProjectPersistedInDb()
        {
            var store = Configure.OpenStore();
            Configure.Populate(store, Reset);

            using (var session = store.OpenSession())
            {
                var controller = ControllerForTests<ProjectsController>(session, HttpMethod.Delete);
                controller.Delete(1);

                session.SaveChanges();
            }

            using (var session = store.OpenSession())
            {
                Assert.Null(session.Load<Project>(1));
            }
        }

        public static void Reset(IDocumentSession session)
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
    }
}