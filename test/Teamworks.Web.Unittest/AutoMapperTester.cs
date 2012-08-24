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
        public void CanMapProjectToProjectViewModel()
        {
            var comment = new Project();
            Assert.DoesNotThrow(() => comment.MapTo<ProjectViewModel>());
            Assert.DoesNotThrow(() => comment.MapTo<ViewModels.Api.ProjectViewModel>());
        }

        [Fact]
        public void CanMapActivityToActivityViewModel()
        {
            var comment = new Activity();
            Assert.DoesNotThrow(() => comment.MapTo<ActivityViewModel>());
            Assert.DoesNotThrow(() => comment.MapTo<ViewModels.Api.ActivityViewModel>());
        }
    }
}
