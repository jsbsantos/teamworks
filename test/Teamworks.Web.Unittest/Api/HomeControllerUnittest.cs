using System.Collections.Generic;
using Teamworks.Core;
using Teamworks.Core.Services;
using Teamworks.Web.Controllers.Api;
using Teamworks.Web.Uni.Api.Fixture;
using Xunit;
using Activity = Teamworks.Web.Models.Api.Activity;

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
            Fixture.InjectPersonAsCurrentIdentity(new Person
                                                      {
                                                          Id = "people/1",
                                                          Name = "Something",
                                                          Username = "somthing"
                                                      });
            using (IDocumentSession session = Global.Database.OpenSession())
            {
                var home = new HomeController(session);
                IEnumerable<Activity> result = home.GetActivities();
            }
        }
    }
}