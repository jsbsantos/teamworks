using System.Linq;
using Teamworks.Core;
using Teamworks.Core.Services;
using Teamworks.Web.Controllers.Api;
using Teamworks.Web.Uni.Api.Fixture;
using Xunit;

namespace Teamworks.Web.Uni.Api
{
    public class DiscussionsControllerUnittest: IUseFixture<DocumentStoreFixture>
    {
        public DocumentStoreFixture Fixture { get; set; }

        [Fact]
        public void GetDiscussions()
        {
            using (var session = Global.Database.OpenSession())
            {
                var controller = new DiscussionsController(session);
                var discussions = controller.Get(1).ToList();
                
                Assert.NotNull(discussions);
                Assert.Equal(3, discussions.Count());
            }
        }

        public void SetFixture(DocumentStoreFixture data)
        {
            Fixture = data;
            Fixture.Initialize();
        }
    }
}
