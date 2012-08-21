using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AttributeRouting;
using AttributeRouting.Web.Mvc;
using Teamworks.Core;
using Teamworks.Core.Services;
using Teamworks.Web.Attributes.Mvc;
using Teamworks.Web.Helpers;
using Teamworks.Web.Helpers.AutoMapper;
using Teamworks.Web.Helpers.Extensions.Mvc;
using Teamworks.Web.ViewModels.Mvc;

namespace Teamworks.Web.Controllers.Mvc
{
    [SecureProject("projects/view/discussions/view")]
    [RoutePrefix("projects/{projectId}/discussions")]
    public class DiscussionsController : RavenController
    {
        [GET("")]
        [GET("projects/{projectId}/activities/{activityId}/discussions"
            , IsAbsoluteUrl = true, RouteName = "discussions_activitiesget")]
        public ActionResult Get(int projectid, int? activityId)
        {
            return HttpNotFound();
        }

        [GET("{discussionId}")]
        [GET("projects/{projectId}/activities/{activityId}/discussions/{discussionId}"
            , IsAbsoluteUrl = true, RouteName = "discussions_activitiesdetails")]
        public ActionResult Details(int projectId, int? activityId, int discussionId)
        {
            Entity entity = DbSession.Load<Project>(projectId);
            var discussion = DbSession
                .Include<Discussion>(d => d.Messages.SelectMany(m => m.Person))
                .Load<Discussion>(discussionId);

            if (activityId.HasValue)
                entity = DbSession.Load<Activity>(activityId);

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

        [POST("")]
        public ActionResult Post(int projectId, DiscussionViewModel.Input model)
        {
            if (!ModelState.IsValid)
                return View("View");

            var personId = DbSession.GetCurrentPersonId();
            var project = DbSession.Load<Project>(projectId);
            var discussion = Discussion.Forge(model.Name, model.Content, project.Id, personId);

            DbSession.Store(discussion);

            var discussionViewModel = discussion.MapTo<DiscussionViewModel>();
            if (Request.IsAjaxRequest())
                return new JsonNetResult {Data = discussionViewModel};
            return RedirectToRoute("discussions_get");
        }

        [POST("{discussionid}/delete")]
        public ActionResult Delete(int projectId, int discussionid)
        {
            if (!ModelState.IsValid)
                return View("View");

            var discussion = DbSession.Load<Discussion>(discussionid);
            if (discussion.Entity.ToIdentifier() != projectId)
                return HttpNotFound();

            DbSession.Delete(discussion);
            return new HttpStatusCodeResult(HttpStatusCode.NoContent);
        }

        [POST("{discussionid}/watch")]
        [SecureProject("projects/view/discussions/view")]
        public ActionResult Watch(int projectId, int discussionid)
        {
            if (!ModelState.IsValid)
                return View("View");

            var personId = DbSession.GetCurrentPersonId();
            var discussion = DbSession.Load<Discussion>(discussionid);
            if (!discussion.Subscribers.Contains(personId))
                discussion.Subscribers.Add(personId);

            return  new HttpStatusCodeResult(HttpStatusCode.Created);
        }

        [POST("{discussionid}/unwatch")]
        [SecureProject("projects/view/discussions/view")]
        public ActionResult Unwatch(int projectId, int discussionid)
        {
            if (!ModelState.IsValid)
                return View("View");

            var personId = DbSession.GetCurrentPersonId();
            var discussion = DbSession.Load<Discussion>(discussionid);
            if (discussion.Subscribers.Contains(personId))
                discussion.Subscribers.Remove(personId);

            return new HttpStatusCodeResult(HttpStatusCode.NoContent);
        }

        [AjaxOnly]
        [POST("{discussionId}/messages")]
        public ActionResult PostMessage(int projectId, int discussionId, string content = "")
        {
            content = content.Trim();
            if (string.IsNullOrEmpty(content))
                ModelState.AddModelError("model.message", "Your message cannot be empty.");

            if (!ModelState.IsValid)
                throw new HttpException((int) HttpStatusCode.BadRequest, ModelState.Values.First().ToString());

            var project = DbSession.Load<Project>(projectId);
            var discussion = DbSession.Load<Discussion>(discussionId);

            if (discussion == null || discussion.Entity != project.Id)
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
        [POST("{discussionId}/messages/{messageId}/delete")]
        public ActionResult DeleteMessage(int projectId, int discussionId, int messageId)
        {
            var project = DbSession.Load<Project>(projectId);
            var discussion = DbSession.Load<Discussion>(discussionId);

            if (discussion == null || discussion.Entity != project.Id)
                return new HttpNotFoundResult();

            var message = discussion.Messages.FirstOrDefault(m => m.Id == messageId);
            if (message != null)
            {
                if (message.Person == DbSession.GetCurrentPersonId())
                    discussion.Messages.Remove(message);
                else
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }

            return new HttpStatusCodeResult(HttpStatusCode.NoContent);
        }

        [NonAction]
        public override BreadcrumbViewModel[] CreateBreadcrumb()
        {
            var activityId = 0;
            if (RouteData.Values.ContainsKey("activityId"))
                activityId = int.Parse(RouteData.Values["activityId"].ToString());

            var projectId = int.Parse(RouteData.Values["projectId"].ToString());
            var discussionId = int.Parse(RouteData.Values["discussionId"].ToString());


            var project = DbSession.Load<Project>(projectId);
            var discussion = DbSession.Load<Discussion>(discussionId);

            var breadcrumb = new List<BreadcrumbViewModel>
                {
                    new BreadcrumbViewModel
                        {
                            Url = Url.RouteUrl("projects_get"),
                            Name = "Projects"
                        },
                    new BreadcrumbViewModel
                        {
                            Url = Url.RouteUrl("projects_get", new {projectId}),
                            Name = project.Name
                        }
                };
            
            string get;
            string details;
            if (activityId > 0)
            {
                var activity = DbSession.Load<Activity>(activityId);
                breadcrumb.Add(new BreadcrumbViewModel
                    {
                        Url = Url.RouteUrl("activities_details", new {projectId, activityId = UrlParameter.Optional}),
                        Name = "Activities"
                    });

                breadcrumb.Add(new BreadcrumbViewModel
                    {
                        Url = Url.RouteUrl("activities_details", new {projectId, activityId}),
                        Name = activity.Name
                    });

                get = Url.RouteUrl("discussions_activitiesget", new { projectId, activityId });
                details = Url.RouteUrl("discussions_activitiesdetails", new {projectId, activityId, discussionId});
            }
            else
            {
                get = Url.RouteUrl("discussions_activitiesget", new {projectId, activityId});
                details = Url.RouteUrl("discussions_activitiesdetails", new {projectId, activityId, discussionId});
            }
            breadcrumb.Add(new BreadcrumbViewModel
                {
                    Url = get,
                    Name = "Discussions"
                });
            breadcrumb.Add(new BreadcrumbViewModel
                {
                    Url = details,
                    Name = discussion.Name
                });
            
            return breadcrumb.ToArray();
        }
    }
}