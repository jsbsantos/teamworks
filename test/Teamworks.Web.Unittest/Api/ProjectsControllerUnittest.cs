using System.Linq;
using Teamworks.Core;
using Teamworks.Core.Services;
using Teamworks.Web.Controllers.Api;
using Teamworks.Web.Uni.Api.Fixture;
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
            using (IDocumentSession session = Global.Database.OpenSession())
            {
                var controller = new ProjectsController(session);
                int size = controller.Get().Count();
                Fixture.Store(Project.Forge("proj 1", "proj 1 description"));

                Assert.Equal(size, controller.Get().Count());
            }
        }

        [Fact]
        public void GetProjectById()
        {
            string name = "proj 1";
            string description = "description 1";
            var controller = new ProjectsController();
            Project project = Project.Forge(name, description);
            Fixture.Store(project);

            Models.Api.Project result = controller.Get(project.Identifier);
            Assert.NotNull(result);
            Assert.Equal(project.Name, result.Name);
            Assert.Equal(project.Description, result.Description);
        }

        [Fact]
        public void PostProject()
        {
            /*
            var name = "proj 1";
            var description = "description 1";

            var controller = new ProjectsController();
            var project = controller.Post(new Project
                                {
                                    Name = name,
                                    Description = description
                                }).Content.ReadAsAsync<Project>().Result;

            var result = Fixture.Load<Core.Project>(project.Id);
            Assert.NotNull(result);
            Assert.Equal(project.Name, result.Name);
            Assert.Equal(project.Description, result.Description);
            Assert.False(result.Archived);
            */
        }
    }
}