using System.Web.Mvc;
using AttributeRouting;
using AttributeRouting.Web.Mvc;
using Teamworks.Core;
using Teamworks.Core.Services;
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
            var discussion = DbSession.Load<Discussion>(discussionId);
            if (discussion == null || discussion.Entity.Equals(projectId.ToId("person")))
                return HttpNotFound();

            var project = DbSession.Load<Project>(projectId);
            var discussionViewModel = discussion.MapTo<DiscussionViewModel>();
            
            return View(discussionViewModel);
        }

        [POST("")]
        [SecureProject("projects/view/discussions/create")]
        public ActionResult Post(int projectId, DiscussionViewModel.Input model)
        {
            if (!ModelState.IsValid)
                return View("View");

            var personId = HttpContext.GetCurrentPersonId();
            var project = DbSession.Load<Project>(projectId);
            var discussion = Discussion.Forge(model.Name, model.Content, project.Id, personId);

            DbSession.Store(discussion);

            var discussionViewModel = discussion.MapTo<DiscussionViewModel>();
            if (Request.IsAjaxRequest())
                return new JsonNetResult {Data = discussionViewModel};
            return RedirectToRoute("discussions_get");
        }
    }
}