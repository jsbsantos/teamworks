using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AttributeRouting;
using AttributeRouting.Web.Http;
<<<<<<< Updated upstream
using AutoMapper;
=======
>>>>>>> Stashed changes
using Raven.Client.Linq;
using Teamworks.Core;
using Teamworks.Core.Services;
using Teamworks.Web.Attributes.Api;
using Teamworks.Web.Helpers.AutoMapper;
using Teamworks.Web.Helpers.Extensions.Api;
using Teamworks.Web.ViewModels.Api;

namespace Teamworks.Web.Controllers.Api
{
    [SecureProject("projects/view")]
    [RoutePrefix("api/projects/{projectId}")]
    public class DiscussionsController : RavenApiController
    {
        [GET("discussions")]
        [GET("activities/{activityId}/discussions", RouteName = "api_discussions_getactivities")]
        public IEnumerable<DiscussionViewModel> Get(int projectId, int? activityId)
        {
            var entity = activityId.HasValue
                             ? activityId.Value.ToId("activity")
                             : projectId.ToId("project");

            var discussions = DbSession.Query<Discussion>()
                .Where(d => d.Entity == entity);

            return discussions.MapTo<DiscussionViewModel>();
        }

        [GET("discussions/{id}")]
<<<<<<< Updated upstream
        [GET("activities/{activityId}/discussions/{id}", RouteName = "api_discussions_get_byidactivities")]
        public Discussion GetById(int id, int projectId, int? activityId)
=======
        [GET("activities/{activityId}/discussions", RouteName = "api_discussions_getbyidactivities")]
        public DiscussionViewModel GetById(int id, int projectId, int? activityId)
>>>>>>> Stashed changes
        {
            var entity = activityId.HasValue
                             ? activityId.Value.ToId("activity")
                             : projectId.ToId("project");

<<<<<<< Updated upstream
            var discussion = DbSession
                .Query<Discussion>()
                .FirstOrDefault(a => a.Id == id.ToId("discussion"));

            if (discussion == null || discussion.Entity != entity)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            return Mapper.Map<Discussion, Discussion>(discussion);
=======
            if (activityId.HasValue)
            {
                var activity = DbSession.Load<Activity>(activityId);
                if (activity != null && activity.Project == projectId.ToId("project"))
                    throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            var discussion = DbSession.Load<Discussion>(id);
            if (discussion.Entity != entity)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            return discussion.MapTo<DiscussionViewModel>();
>>>>>>> Stashed changes
        }

        [POST("discussions")]
        [POST("activities/{activityId}/discussions", RouteName = "api_discussions_postactivities")]
<<<<<<< Updated upstream
        public HttpResponseMessage Post(int projectId, Discussion model, int? activityId)
        {
            string entity = activityId.HasValue
                                ? activityId.Value.ToId("activity")
                                : projectId.ToId("project");

            if ((activityId.HasValue && DbSession.Load<Activity>(entity) == null) ||
                DbSession.Load<Project>(entity) == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            Discussion discussion = Discussion.Forge(model.Name, model.Content, entity,
                                                     Request.GetCurrentPersonId());

            DbSession.Store(discussion);
            Discussion response = Mapper.Map<Discussion, Discussion>(discussion);
=======
        public HttpResponseMessage Post(int projectId, int? activityId, Discussion model)
        {
            var entity = activityId.HasValue
                             ? activityId.Value.ToId("activity")
                             : projectId.ToId("project");

            if (activityId.HasValue)
            {
                var activity = DbSession.Load<Activity>(activityId);
                if (activity != null && activity.Project == projectId.ToId("project"))
                    throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            var discussion = Discussion.Forge(
                model.Name,
                model.Content,
                entity,
                Request.GetCurrentPersonId());

            DbSession.Store(discussion);
            var value = discussion.MapTo<DiscussionViewModel>();

            object values;
            if (activityId.HasValue)
            {
                values = new {projectId, activityId, id = discussion.Id};
            }
            else
            {
                values = new {projectId, id = discussion.Id};
            }

            var uri = Url.Link("api_discussions_post", values);
            var response = Request.CreateResponse(HttpStatusCode.Created, value);
            response.Headers.Location = new Uri(uri);
            return response;
        }
>>>>>>> Stashed changes

            return Request.CreateResponse(HttpStatusCode.Created, response);
        }

        [DELETE("discussions/{id}")]
        [DELETE("activities/{activityId}/discussions/{id}", RouteName = "api_discussions_deleteactivities")]
        public HttpResponseMessage Delete(int id, int projectId, int? activityId)
        {
<<<<<<< Updated upstream
            string entity = activityId.HasValue
                                ? activityId.Value.ToId("activity")
                                : projectId.ToId("project");
=======
            var discussion = DbSession
                .Query<Discussion>()
                .FirstOrDefault(a => a.Entity == projectId.ToId("project")
                                     && a.Id == id.ToId("discussion"));
>>>>>>> Stashed changes

            var discussion = DbSession
                .Query<Discussion>()
                .FirstOrDefault(a => a.Id == id.ToId("discussion"));

            if (discussion != null && discussion.Entity == entity)
                DbSession.Delete(discussion);

            return Request.CreateResponse(HttpStatusCode.NoContent);
        }

<<<<<<< Updated upstream
        [GET("discussions/{id}/messages")]
        [GET("activities/{activityId}/discussions/{id}/messages", RouteName = "api_discussions_messages_getactivities")]
        public IEnumerable<MessageViewModel> GetProjectMessages(int id, int projectId, int? activityId)
        {
            string entity = activityId.HasValue
                                ? activityId.Value.ToId("activity")
                                : projectId.ToId("project");

            var discussion = DbSession
                .Query<Discussion>()
                .FirstOrDefault(a => a.Id == id.ToId("discussion"));

            if (discussion != null || discussion.Entity == entity)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            return discussion.Messages.MapTo<MessageViewModel>();
        }

        //todo test route
        [POST("discussions/{id}/messages")]
        [POST("activities/{activityId}/discussions/{id}/messages", RouteName = "api_discussions_messages_postactivities")]
        public HttpResponseMessage PostProjectMessages(int id, int projectId, int? activityId, MessageViewModel model)
        {
            string entity = activityId.HasValue
                                ? activityId.Value.ToId("activity")
                                : projectId.ToId("project");

            var discussion = DbSession
                .Query<Discussion>()
                .FirstOrDefault(a => a.Id == id.ToId("discussion"));

            if (discussion != null || discussion.Entity == entity)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            string current = Request.GetCurrentPersonId();
            var message = Discussion.Message.Forge(model.Content, current);

            message.Id = discussion.GenerateNewMessageId();

            discussion.Messages.Add(message);

            var value = message.MapTo<MessageViewModel>();
            return Request.CreateResponse(HttpStatusCode.Created, value);
        }

        [DELETE("discussions/{discussionId}/messages/{id}")]
        [DELETE("activities/{activityId}/discussions/{discussionId}/messages/{id}",
            RouteName = "api_discussions_messages_deleteactivities")]
        public HttpResponseMessage DeleteProjectMessages(int id, int discussionId, int projectId, int? activityId)
        {
            string entity = activityId.HasValue
                                ? activityId.Value.ToId("activity")
                                : projectId.ToId("project");

            var discussion = DbSession
                .Query<Discussion>()
                .FirstOrDefault(a => a.Id == discussionId.ToId("discussion"));

            if (discussion != null || discussion.Entity == entity)
                throw new HttpResponseException(HttpStatusCode.NoContent);

            Discussion.Message message = discussion.Messages.FirstOrDefault(m => m.Id == id);

            if (message != null)
                discussion.Messages.Remove(message);

            return Request.CreateResponse(HttpStatusCode.NoContent);
        }
=======
        #endregion
>>>>>>> Stashed changes
    }
}