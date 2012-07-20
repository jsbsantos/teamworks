using Xunit;
using Teamworks.Web.Controllers.Api;

namespace Teamworks.Web.Test.Api
{
    public class ProjectsControllerTest
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
    }

}
