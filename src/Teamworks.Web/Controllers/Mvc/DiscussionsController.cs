using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AttributeRouting;
using AttributeRouting.Web.Mvc;
using Raven.Client.Util;
using Teamworks.Core;
using Teamworks.Core.Services;
using Teamworks.Web.Attributes.Mvc;
using Teamworks.Web.Helpers;
using Teamworks.Web.Helpers.AutoMapper;
using Teamworks.Web.Helpers.Extensions.Mvc;
using Teamworks.Web.ViewModels.Mvc;

namespace Teamworks.Web.Controllers.Mvc
{
    [RoutePrefix("projects/{projectId}/discussions")]
    public class DiscussionsController : RavenController
    {
        [GET("")]
        [SecureProject("projects/view/discussions/view")]
        public ActionResult Get(int projectid)
        {
            return HttpNotFound();
        }

        [GET("{discussionId}")]
        [SecureProject("projects/view/discussions/view")]
        public ActionResult Details(int projectId, int discussionId)
        {
            var project = DbSession.Load<Project>(projectId);
            var discussion = DbSession
                .Include<Discussion>(d => d.Messages.SelectMany(m => m.Person))
                .Load<Discussion>(discussionId);

            if (discussion == null || !discussion.Entity.Equals(project.Id))
                return HttpNotFound();

            var discussionViewModel = discussion.MapTo<DiscussionViewModel>();
            discussionViewModel.Project = project.MapTo<EntityViewModel>();

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
        [SecureProject("projects/view/discussions/create")]
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
        [SecureProject("projects/view/discussions/create")]
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
        [SecureProject("projects/view/discussions/view")]
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
        [SecureProject("projects/view/discussions/view")]
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
    }
}