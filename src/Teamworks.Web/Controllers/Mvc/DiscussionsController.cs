using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AttributeRouting;
using AttributeRouting.Web.Mvc;
using Teamworks.Core;
using Teamworks.Web.Attributes.Mvc;
using Teamworks.Web.Helpers.AutoMapper;
using Teamworks.Web.Helpers.Extensions.Mvc;
using Teamworks.Web.ViewModels.Api;
using Teamworks.Web.Views;

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
            foreach (var message in discussion.Messages)
            {
                var messageViewModel = message.MapTo<DiscussionViewModel.Message>();
                messageViewModel.Person = DbSession.Load<Person>(message.Person)
                    .MapTo<PersonViewModel>();

                discussionViewModel.Messages.Add(messageViewModel);
            }
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

        [AjaxOnly]
        [POST("{discussionId}/messages/create")]
        [SecureProject("projects/view/discussions/view")]
        public ActionResult PostMessage(int projectId, int discussionId, string content = "")
        {
            content = content.Trim();
            if(string.IsNullOrEmpty(content))
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

            return new JsonNetResult() {Data = messageViewModel};
        }

        [AjaxOnly]
        [POST("{discussionId}/messages/delete/{messageId}")]
        [SecureProject("projects/view/discussions/view")]
        public ActionResult DeleteMessage(int projectId, int discussionId, int messageId)
        {
            var project = DbSession.Load<Project>(projectId);
            var discussion = DbSession.Load<Discussion>(discussionId);

            if (discussion == null || discussion.Entity != project.Id)
                return new HttpNotFoundResult();

            var message = discussion.Messages.Where(m => m.Id == messageId).FirstOrDefault();
            if (message != null)
                discussion.Messages.Remove(message);

            return new HttpStatusCodeResult(HttpStatusCode.NoContent);
        }

    }
}