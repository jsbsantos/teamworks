using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using AttributeRouting;
using AttributeRouting.Web.Http;
using AutoMapper;
using Teamworks.Core;
using Teamworks.Core.Services;
using Teamworks.Web.Helpers.Extensions.Api;
using Message = Teamworks.Web.ViewModels.Api.Message;

namespace Teamworks.Web.Controllers.Api
{
    [DefaultHttpRouteConvention]
    [RoutePrefix("api/projects/{projectId}")]
    public class MessagesController : RavenApiController
    {
        public Discussion GetDiscussion(int discussionId, string entity)
        {
            Discussion discussion = DbSession.Query<Discussion>()
                .FirstOrDefault(d => d.Entity == entity
                                     && d.Id == discussionId.ToId("discussion"));

            Request.NotFound(discussion);
            return discussion;
        }

        [GET("discussions/{discussionId}/messages")]
        public IEnumerable<Message> GetMessageFromProjectDiscussion(int projectId, int discussionId)
        {
            IList<Core.Message> messages = GetDiscussion(discussionId, projectId.ToId("project")).Messages;
            return Mapper.Map<IEnumerable<Core.Message>, IEnumerable<Message>>(messages);
        }

        [POST("discussions/{discussionId}/messages")]
        public HttpResponseMessage PostMessageOnProjectDiscussion(
            int discussionId, int projectId, Message model)
        {
            Discussion discussion = GetDiscussion(discussionId, projectId.ToId("project"));

            string current = Request.GetCurrentPersonId();
            Core.Message message = Core.Message.Forge(model.Content, current);

            message.Id = discussion.GenerateNewTimeEntryId();

            discussion.Messages.Add(message);
            //discussion.Notify(message, DbSession.Load<Core.Person>(discussion.Subscribers)
            //            .Select(x => x.Email).ToList());

            Message value = Mapper.Map<Core.Message, Message>(message);
            return Request.CreateResponse(HttpStatusCode.Created, value);
        }

        [DELETE("discussions/{discussionid}/messages/{id}")]
        public HttpResponseMessage DeleteMessageFromProjectDiscussion(int id, int projectId, int discussionId)
        {
            IList<Core.Message> messages = GetDiscussion(discussionId, projectId.ToId("project")).Messages;
            Core.Message message = messages.FirstOrDefault(m => m.Id == id);
            Request.NotFound(message);

            messages.Remove(message);
            return Request.CreateResponse(HttpStatusCode.NoContent);
        }

        #region ActivityViewModel

/*
        private Core.Discussion LoadTaskDiscussion(int projectid, int activityid, int discussionid)
        {
            var project = DbSession
                .Include<Core.ProjectViewModel>(p => p.Activities)
                .Load<Core.ProjectViewModel>(projectid);

            if (project == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            var task = DbSession
                .Include<Core.ActivityViewModel>(t => t.Messages)
                .Load<Core.ActivityViewModel>(activityid);

            if (task == null || !task.ProjectViewModel.Equals(project.Id))
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            var discussion = DbSession.Load<Core.Discussion>(discussionid);
            if (discussion == null || !discussion.EntityId.Equals(task.Id))
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return discussion;
        }

        [GET("activities/{activityid}/discussions/{discussionid}/messages")]
        public IEnumerable<Message> GetTaskDiscussionMessage(int projectid, int activityid, int discussionid)
        {
            return
                LoadTaskDiscussion(projectid, activityid, discussionid).Messages.Select(
                    Mapper.Map<Core.Message, Message>);
        }

        [POST("activities/{activityid}/discussions/{discussionid}/messages")]
        public HttpResponseMessage PostTaskDiscussionMessage(int discussionid, int projectid, int activityid,
                                                             Message model)
        {
            var discussion = LoadTaskDiscussion(projectid, activityid, discussionid);

            var message = Core.Message.Forge(model.Content, Request.GetCurrentPersonId());
            message.Id = discussion.GenerateNewTimeEntryId();
            discussion.Messages.Add(message);

            var value = Mapper.Map<Core.Message, Message>(message);
            return Request.CreateResponse(HttpStatusCode.Created, value);
        }

        [DELETE("activities/{activityid}/discussions/{discussionid}/messages/{id}")]
        public HttpResponseMessage DeleteTaskDiscussionMessage(int id, int projectid, int activityid, int discussionid)
        {
            var discussion = LoadTaskDiscussion(projectid, activityid, discussionid);

            var message = discussion.Messages.FirstOrDefault(m => m.Id == id);
            if (message == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            discussion.Messages.Remove(message);
            return Request.CreateResponse(HttpStatusCode.NoContent);
        }
        */

        #endregion
    }
}