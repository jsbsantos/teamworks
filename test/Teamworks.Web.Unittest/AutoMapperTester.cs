using Teamworks.Core;
using Teamworks.Web.Helpers.AutoMapper;
using Teamworks.Web.ViewModels.Mvc;
using Xunit;

namespace Teamworks.Web.Unittest
{
    public class AutoMapperTester
    {
        public AutoMapperTester()
        {
            AutoMapperConfiguration.Configure();
        }

        [Fact]
        public void CanMapFromProjectToProjectViewModel()
        {
            var comment = new Project();
            Assert.DoesNotThrow(() => comment.MapTo<ProjectViewModel>());
        }
    }
}
