using Xunit;

namespace Teamworks.Web.Test.Api
{
    public class ProjectsController
    {
        [Fact]
        public void GetProjects()
        {
            var controller = new Controllers.Api.ProjectsController();
            var result = controller.Get();
        }
    }

}
