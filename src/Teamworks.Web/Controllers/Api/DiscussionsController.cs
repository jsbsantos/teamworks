using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AttributeRouting;
using AttributeRouting.Web.Http;
using AutoMapper;
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
        [GET("activities/{activityId}/discussions/{id}", RouteName = "api_discussions_get_byidactivities")]
        public Discussion GetById(int id, int projectId, int? activityId)
        {
            var entity = activityId.HasValue
                             ? activityId.Value.ToId("activity")
                             : projectId.ToId("project");

            var discussion = DbSession
                .Query<Discussion>()
                .FirstOrDefault(a => a.Id == id.ToId("discussion"));

            if (discussion == null || discussion.Entity != entity)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            return Mapper.Map<Discussion, Discussion>(discussion);
        }

        [POST("discussions")]
        [POST("activities/{activityId}/discussions", RouteName = "api_discussions_postactivities")]
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

            return Request.CreateResponse(HttpStatusCode.Created, response);
        }

        [DELETE("discussions/{id}")]
        [DELETE("activities/{activityId}/discussions/{id}", RouteName = "api_discussions_deleteactivities")]
        public HttpResponseMessage Delete(int id, int projectId, int? activityId)
        {
            string entity = activityId.HasValue
                                ? activityId.Value.ToId("activity")
                                : projectId.ToId("project");

            var discussion = DbSession
                .Query<Discussion>()
                .FirstOrDefault(a => a.Id == id.ToId("discussion"));

            if (discussion != null && discussion.Entity == entity)
                DbSession.Delete(discussion);

            return Request.CreateResponse(HttpStatusCode.NoContent);
        }

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
    }
}