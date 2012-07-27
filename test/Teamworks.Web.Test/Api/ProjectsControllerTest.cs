using Raven.Client.Document;
using Xunit;
using Teamworks.Web.Controllers.Api;

namespace Teamworks.Web.Test.Api
{
    public class ProjectsControllerTest : IUseFixture<EmbeddableDocumentStoreFixture>
    {
        [Fact]
        public void GetProjects()
        {
            var controller = new ProjectsController();
            var result = controller.Get();
            Assert.NotNull(result);
            foreach (var project in result)
            {
                Assert.NotNull(project.Id);
            }
        }

        public void SetFixture(EmbeddableDocumentStoreFixture data)
        {
            data.Initialize();
        }
    }
}