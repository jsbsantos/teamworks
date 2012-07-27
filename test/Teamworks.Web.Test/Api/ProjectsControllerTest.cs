using Raven.Client.Document;
using Raven.Client.Embedded;
using Teamworks.Core.Services.RavenDb;
using Xunit;
using Teamworks.Web.Controllers.Api;

namespace Teamworks.Web.Test.Api
{
    public class ProjectsControllerTest : IUseFixture<EmbeddableDatabase>
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

        public void SetFixture(EmbeddableDatabase data)
        {
            data.Initialize();
        }
    }

    public class EmbeddableDatabase
    {
        public void Initialize()
        {
            Session.Store =
                new EmbeddableDocumentStore
                    {
                        UseEmbeddedHttpServer = true
                    }.RegisterListener(new PersonQueryListenter())
                    .Initialize();
        }
    }
}