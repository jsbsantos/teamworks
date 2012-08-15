using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Hosting;
using System.Web.Http.Routing;
using Raven.Client;
using Teamworks.Core;
using Teamworks.Core.Services;
using Teamworks.Web.Controllers.Api;
using Teamworks.Web.Uni.Api.Fixture;
using Teamworks.Web.ViewModels.Api;
using Xunit;

namespace Teamworks.Web.Uni.Api
{
    public class ProjectsControllerUnittest : IUseFixture<DocumentStoreFixture>
    {
        public DocumentStoreFixture Fixture { get; set; }

        #region IUseFixture<DocumentStoreFixture> Members

        public void SetFixture(DocumentStoreFixture data)
        {
            Fixture = data;
            Fixture.Initialize();
        }

        #endregion

        [Fact]
        public void GetProjects()
        {
            var size = 0;
            using (var session = Global.Database.OpenSession())
            {
                size = session.Query<Project>().Count() + 1;
            }

            var project = Project.Forge("proj 1", "description 1");
            Fixture.Store(project);
            using (var session = Global.Database.OpenSession())
            {
                var controller = new ProjectsController(session);
                var result = controller.Get().ToList();
                Assert.Equal(size, result.Count());

                foreach (var model in result)
                {
                    Assert.NotNull(model);
                }
            }
        }

        [Fact]
        public void GetProjectById()
        {
            const string name = "proj 1";
            const string description = "description 1";

            var project = Project.Forge(name, description);
            Fixture.Store(project);

            using (var session = Global.Database.OpenSession())
            {
                var controller = new ProjectsController(session);
                var result = controller.Get(project.Identifier);

                Assert.NotNull(result);
                Assert.Equal(project.Name, result.Name);
                Assert.Equal(project.Description, result.Description);
            }
        }

        [Fact]
        public void PostProjectReturnsCreatedStatusCode()
        {
            const string name = "proj 1";
            const string description = "description 1";

            var person = Person.Forge("email@mail.pt", "username", "password", "Name");

            Fixture.InjectPersonAsCurrentIdentity(person);

            ProjectViewModel project;
            HttpResponseMessage response;
            using (var session = Global.Database.OpenSession())
            {
                var controller = ControllerForTests(new ProjectsController(session));
                response = controller.Post(new ProjectViewModel
                {
                    Name = name,
                    Description = description
                });
                session.SaveChanges();
                project = response.Content.ReadAsAsync<ProjectViewModel>().Result;
            }
            
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.Equal(project.Name, name);
            Assert.Equal(project.Description, description);

            var result = Fixture.Load<Project>(project.Id);

            Assert.NotNull(result);
            Assert.Equal(result.Name, name);
            Assert.Equal(result.Description, description);
            Assert.False(result.Archived);
        }

        public static T ControllerForTests<T>(T t) where T: ApiController
        {
            var config = new HttpConfiguration();
            var request = new HttpRequestMessage(HttpMethod.Post, "http://localhost/api/projects");
            var route = config.Routes.MapHttpRoute("default", "api/{projects}/{id}");
            var routeData = new HttpRouteData(route, new HttpRouteValueDictionary { { "controller", "projects" } });

            t.ControllerContext = new HttpControllerContext(config, routeData, request);
            t.Request = request;
            t.Request.Properties[HttpPropertyKeys.HttpConfigurationKey] = config;
            return t;
        }
    }
}