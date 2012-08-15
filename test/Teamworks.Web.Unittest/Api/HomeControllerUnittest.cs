using Teamworks.Web.Uni.Api.Fixture;
using Xunit;

namespace Teamworks.Web.Uni.Api
{
    public class HomeControllerUnittest : IUseFixture<DocumentStoreFixture>
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
        public void GetActivitiesForCurrentUser()
        {
           
        }
    }
}