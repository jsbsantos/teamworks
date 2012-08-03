﻿using System.Net.Http;
using Xunit;
using Teamworks.Core;
using Teamworks.Core.Services;
using Teamworks.Web.Test.Api.Fixture;
using Teamworks.Web.Controllers.Api;

namespace Teamworks.Web.Test.Api
{
    public class HomeControllerUnittest : IUseFixture<DocumentStoreFixture>
    {
        public DocumentStoreFixture Fixture { get; set; }

        [Fact]
        public void GetActivitiesForCurrentUser()
        {
            Fixture.InjectPersonAsCurrentIdentity(new Person {
                                                          Id = "people/1",
                                                          Name = "Something",
                                                          Username = "somthing"
                                                      });
            using (var session = Global.Store.OpenSession())
            {
                var home = new HomeController(session);
                var result = home.GetActivities();

            }
        }

        public void SetFixture(DocumentStoreFixture data)
        {
            Fixture = data;
            Fixture.Initialize();
        }
    }
}