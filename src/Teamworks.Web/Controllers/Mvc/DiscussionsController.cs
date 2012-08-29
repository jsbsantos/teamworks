using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using AttributeRouting;
using AttributeRouting.Web.Mvc;
using Teamworks.Core;
using Teamworks.Core.Extensions;
using Teamworks.Core.Services;
using Teamworks.Web.Attributes.Mvc;
using Teamworks.Web.Helpers;
using Teamworks.Web.Helpers.AutoMapper;
using Teamworks.Web.Helpers.Extensions.Mvc;
using Teamworks.Web.ViewModels.Mvc;

namespace Teamworks.Web.Controllers.Mvc
{
    [SecureProject("projects/view/discussions/view")]
    [RoutePrefix("projects/{projectId}")]
    public class DiscussionsController : AppController
    {
        [GET("discussions")]
        [GET("activities/{activityId}/discussions", RouteName = "discussions_activitiesget")]
        public ActionResult Get(int projectid, int? activityId)
        {
            throw new HttpResponseException(HttpStatusCode.NotFound);
        }

        [GET("discussions/{discussionId}")]
        [GET("activities/{activityId}/discussions/{discussionId}", RouteName = "discussions_activitiesdetails")]
        public ActionResult Details(int projectId, int? activityId, int discussionId)
        {
            var entity = activityId.HasValue
                             ? (Entity) DbSession.Load<Activity>(activityId)
                             : DbSession.Load<Project>(projectId);
            if (entity == null)
                return HttpNotFound();

            var discussion = DbSession
                .Include<Discussion>(d => d.Messages.SelectMany(m => m.Person))
                .Load<Discussion>(discussionId);
            if (discussion == null || !discussion.Entity.Equals(entity.Id))
                return HttpNotFound();

            var discussionViewModel = discussion.MapTo<DiscussionViewModel>();
            discussionViewModel.Entity = entity.MapTo<EntityViewModel>();

            var people = new List<PersonViewModel>();
            foreach (var message in discussion.Messages.OrderByDescending(m => m.Date))
            {
                var person = DbSession.Load<Person>(message.Person);
                var messageViewModel = message.MapTo<DiscussionViewModel.Message>();
                messageViewModel.Person = person.MapTo<PersonViewModel>();
                messageViewModel.Editable = person.Id == DbSession.GetCurrentPersonId();
                people.Add(messageViewModel.Person);

                discussionViewModel.Messages.Add(messageViewModel);
            }

            discussionViewModel.People = people.GroupBy(p => p.Id).Select(grp => grp.First()).ToList();

            var personId = DbSession.GetCurrentPersonId();
            discussionViewModel.Watching = discussion.Subscribers.Contains(personId);
            return View(discussionViewModel);
        }

        [POST("discussions")]
        [POST("activities/{activityId}/discussions", RouteName = "discussions_activitiespost")]
        public ActionResult Post(int projectId, int? activityId, DiscussionViewModel.Input model)
        {
            if (!ModelState.IsValid)
                return View("View");

            var entity = activityId.HasValue
                             ? (Entity) DbSession.Load<Activity>(activityId)
                             : DbSession.Load<Project>(projectId);
            if (entity == null)
                return HttpNotFound();

            var personId = DbSession.GetCurrentPersonId();
            var discussion = Discussion.Forge(model.Name, model.Content, entity.Id, personId);

            DbSession.Store(discussion);

            var discussionViewModel = discussion.MapTo<DiscussionViewModel>();
            if (Request.IsAjaxRequest())
                return new JsonNetResult {Data = discussionViewModel};
            return RedirectToRoute("discussions_get");
        }

        [POST("discussions/{discussionId}/delete")]
        [POST("activities/{activityId}/discussions/{discussionId}/delete", RouteName = "discussions_activitiesdelete")]
        public ActionResult Delete(int discussionId, int projectId, int? activityId)
        {
            var entity = activityId.HasValue
                             ? (Entity) DbSession.Load<Activity>(activityId)
                             : DbSession.Load<Project>(projectId);
            if (entity == null)
                return HttpNotFound();

            var discussion = DbSession.Load<Discussion>(discussionId);
            if (discussion != null && discussion.Entity == entity.Id)
                discussion.Delete(DbSession);

            return new HttpStatusCodeResult(HttpStatusCode.NoContent);
        }

        [POST("discussions/{discussionId}/watch")]
        [POST("activities/{activityId}/discussions/{discussionId}/watch", RouteName = "discussions_activitieswatch")]
        [SecureProject("projects/view/discussions/view")]
        public ActionResult Watch(int discussionId, int projectId, int? activityId)
        {
            var personId = DbSession.GetCurrentPersonId();
            var discussion = DbSession.Load<Discussion>(discussionId);
            if (!discussion.Subscribers.Contains(personId))
                discussion.Subscribers.Add(personId);

            return new HttpStatusCodeResult(HttpStatusCode.Created);
        }

        [POST("discussions/{discussionId}/unwatch")]
        [POST("activities/{activityId}/discussions/{discussionId}/unwatch", RouteName = "discussions_activitiesunwatch")
        ]
        [SecureProject("projects/view/discussions/view")]
        public ActionResult Unwatch(int discussionId, int projectId, int? activityId)
        {
            var personId = DbSession.GetCurrentPersonId();
            var discussion = DbSession.Load<Discussion>(discussionId);
            if (discussion.Subscribers.Contains(personId))
                discussion.Subscribers.Remove(personId);

            return new HttpStatusCodeResult(HttpStatusCode.NoContent);
        }

        [AjaxOnly]
        [POST("discussions/{discussionId}/messages")]
        [POST("activities/{activityId}/discussions/{discussionId}/messages",
            RouteName = "discussions_activitiesaddmessage")]
        public ActionResult PostMessage(int discussionId, int projectId, int? activityId, string content = "")
        {
            content = content.Trim();
            if (string.IsNullOrEmpty(content))
                ModelState.AddModelError("model.message", "Your message cannot be empty.");

            if (!ModelState.IsValid)
                throw new HttpException((int) HttpStatusCode.BadRequest, ModelState.Values.First().ToString());

            var entity = activityId.HasValue
                             ? (Entity) DbSession.Load<Activity>(activityId)
                             : DbSession.Load<Project>(projectId);
            if (entity == null)
                return HttpNotFound();

            var discussion = DbSession.Load<Discussion>(discussionId);

            if (discussion == null || discussion.Entity != entity.Id)
                return new HttpNotFoundResult();

            var message = Discussion.Message.Forge(content, DbSession.GetCurrentPersonId());
            message.Id = discussion.GenerateNewMessageId();
            discussion.Messages.Add(message);

            var messageViewModel = message.MapTo<DiscussionViewModel.Message>();
            messageViewModel.Person = DbSession.GetCurrentPerson().MapTo<PersonViewModel>();
            messageViewModel.Editable = true;

            return new JsonNetResult {Data = messageViewModel};
        }

        [AjaxOnly]
        [POST("discussions/{discussionId}/messages/{id}/delete")]
        [POST("activities/{activityId}/discussions/{discussionId}/messages/{id}/delete",
            RouteName = "discussions_activitiesdeletemessage")]
        public ActionResult DeleteMessage(int id, int discussionId, int projectId, int? activityId)
        {
            var entity = activityId.HasValue
                             ? (Entity) DbSession.Load<Activity>(activityId)
                             : DbSession.Load<Project>(projectId);
            if (entity == null)
                return HttpNotFound();

            var discussion = DbSession.Load<Discussion>(discussionId);

            if (discussion != null && discussion.Entity == entity.Id)
            {
                var message = discussion.Messages.FirstOrDefault(m => m.Id == id);
                if (message != null)
                {
                    if (message.Person == DbSession.GetCurrentPersonId())
                        discussion.Messages.Remove(message);
                    else
                        return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                }
            }
            return new HttpStatusCodeResult(HttpStatusCode.NoContent);
        }

        [System.Web.Mvc.NonAction]
        public override Breadcrumb[] CreateBreadcrumb()
        {
            var activityId = 0;
            if (RouteData.Values.ContainsKey("activityId"))
                activityId = int.Parse(RouteData.Values["activityId"].ToString());

            var projectId = int.Parse(RouteData.Values["projectId"].ToString());
            var discussionId = int.Parse(RouteData.Values["discussionId"].ToString());


            var project = DbSession.Load<Project>(projectId);
            var discussion = DbSession.Load<Discussion>(discussionId);

            var breadcrumb = new List<Breadcrumb>
                {
                    new Breadcrumb
                        {
                            Url = Url.RouteUrl("projects_get"),
                            Name = "Projects"
                        },
                    new Breadcrumb
                        {
                            Url = Url.RouteUrl("projects_details", new {projectId}),
                            Name = project.Name
                        }
                };

            string get;
            string details;
            if (activityId > 0)
            {
                var activity = DbSession.Load<Activity>(activityId);
                breadcrumb.Add(new Breadcrumb
                    {
                        Url = Url.RouteUrl("activities_details", new {projectId, activityId = UrlParameter.Optional}),
                        Name = "Activities"
                    });

                breadcrumb.Add(new Breadcrumb
                    {
                        Url = Url.RouteUrl("activities_details"),
                        Name = activity.Name
                    });

                get = Url.RouteUrl("discussions_activitiesget");
                details = Url.RouteUrl("discussions_activitiesdetails");
            }
            else
            {
                get = Url.RouteUrl("discussions_get");
                details = Url.RouteUrl("discussions_details");
            }
            breadcrumb.Add(new Breadcrumb
                {
                    Url = get,
                    Name = "Discussions"
                });
            breadcrumb.Add(new Breadcrumb
                {
                    Url = details,
                    Name = discussion.Name
                });

            return breadcrumb.ToArray();
        }
    }
}