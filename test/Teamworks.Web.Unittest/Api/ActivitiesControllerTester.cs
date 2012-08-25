using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Routing;
using Raven.Client;
using Teamworks.Core;
using Teamworks.Core.Services;
using Teamworks.Web.Controllers.Api;
using Teamworks.Web.ViewModels.Api;
using Xunit;

namespace Teamworks.Web.Unittest.Api
{
    public class ActivitiesControllerTester : BaseControllerTester
    {
        protected override string Url
        {
            get { return "http://localhost/api/projects/1/activities"; }
        }

        protected override IHttpRouteData RouteData(HttpConfiguration config)
        {
            var route = config.Routes.MapHttpRoute("api_activities_getbyid",
                                                   "api/projects/{projectId}/{controller}/{id}");
            return new HttpRouteData(route, new HttpRouteValueDictionary {{"controller", "activities"}});
        }

        [Fact]
        public void GetActivities()
        {
            var store = Configure.OpenStore();
            Configure.Populate(store, Reset);

            const int size = 3;
            const int projectId = 1;

            List<ActivityViewModel> result;
            using (var session = store.OpenSession())
            {
                var controller = ControllerForTests<ActivitiesController>(session, HttpMethod.Get);
                result = controller.Get(projectId).ToList();
            }

            Assert.Equal(size, result.Count());
            Assert.Equal(0, result.Count(s => s == null));
        }

        [Fact]
        public void GetActivityById()
        {
            var store = Configure.OpenStore();
            Configure.Populate(store, Reset);

            const int projectId = 1;
            const int activityId = 1;
            const string name = "act 1";
            const string description = "description 1";

            ActivityViewModel result;
            using (var session = store.OpenSession())
            {
                var controller = ControllerForTests<ActivitiesController>(session, HttpMethod.Get);
                result = controller.GetById(projectId, activityId);
            }

            Assert.NotNull(result);
            Assert.Equal(activityId, result.Id);
            Assert.Equal(projectId, result.Project);

            Assert.Equal(name, result.Name);
            Assert.Equal(description, result.Description);
        }

        [Fact]
        public void PostActivityReturnsCreatedStatusCode()
        {
            var store = Configure.OpenStore();
            Configure.Populate(store, Reset);

            const int projectId = 1;
            const string name = "post activity";
            const string description = "post activity description";
            using (var session = store.OpenSession())
            {
                var controller = ControllerForTests<ActivitiesController>(session, HttpMethod.Post);
                var response = controller.Post(projectId, new CompleteActivityViewModel
                    {
                        Name = name,
                        Description = description
                    });

                session.SaveChanges();
                Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            }
        }

        [Fact]
        public void PostActivityIsPersistedInDb()
        {
            var store = Configure.OpenStore();
            Configure.Populate(store, Reset);

            const int projectId = 1;
            const string name = "post activity";
            const string description = "post activity description";

            ActivityViewModel result;
            using (var session = store.OpenSession())
            {
                var controller = ControllerForTests<ActivitiesController>(session, HttpMethod.Post);
                var response = controller.Post(projectId, new CompleteActivityViewModel()
                    {
                        Name = name,
                        Description = description
                    });
                session.SaveChanges();
                result = response.Content.ReadAsAsync<ActivityViewModel>().Result;
            }

            using (var session = store.OpenSession())
            {
                var activity = session.Load<Activity>(result.Id);

                Assert.NotNull(activity);
                Assert.Equal(name, activity.Name);
                Assert.Equal(description, activity.Description);
                Assert.Equal(projectId, activity.Project.ToIdentifier());
            }
        }

        [Fact]
        public void PostActivityReturnsTheCorrectLocationInResponse()
        {
            var store = Configure.OpenStore();
            Configure.Populate(store, Reset);

            const int projectId = 1;
            const string name = "post activity";
            const string description = "post activity description";

            HttpResponseMessage response;
            using (var session = store.OpenSession())
            {
                var controller = ControllerForTests<ActivitiesController>(session, HttpMethod.Post);
                response = controller.Post(projectId, new CompleteActivityViewModel
                    {
                        Name = name,
                        Description = description
                    });
                session.SaveChanges();
            }

            var activity = response.Content.ReadAsAsync<ActivityViewModel>().Result;
            Assert.Equal("http://localhost/api/projects/" + projectId + "/activities/" + activity.Id,
                         response.Headers.Location.ToString());
        }

        [Fact]
        public void DeleteProjectReturnsNoContentStatusCode()
        {
            var store = Configure.OpenStore();
            Configure.Populate(store, Reset);

            const int projectId = 1;
            const int activityId = 2;

            HttpResponseMessage response;
            using (var session = store.OpenSession())
            {
                var controller = ControllerForTests<ActivitiesController>(session, HttpMethod.Delete);
                response = controller.Delete(projectId, activityId);
                session.SaveChanges();
            }
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        public void DeleteActivityPersistedInDb()
        {
            var store = Configure.OpenStore();
            Configure.Populate(store, Reset);

            const int projectId = 1;
            const int activityId = 2;

            using (var session = store.OpenSession())
            {
                var controller = ControllerForTests<ActivitiesController>(session, HttpMethod.Delete);
                controller.Delete(activityId, projectId);
                session.SaveChanges();
            }

            using (var session = store.OpenSession())
            {
                Assert.Null(session.Load<Activity>(activityId));
            }
        }


        public static void Reset(IDocumentSession session)
        {
            var project = Project.Forge("proj 1", "desc 1");
            session.Store(project);
            foreach (var p in Enumerable.Range(1, 3))
            {
                session.Store(Activity.Forge(project.Id.ToIdentifier(),
                                             "act " + p, "description " + p, 60));
            }
        }
    }
}