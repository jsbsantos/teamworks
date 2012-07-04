using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AttributeRouting;
using AttributeRouting.Web.Http;
using AutoMapper;
using Teamworks.Web.Helpers.Api;
using Teamworks.Web.Helpers.Teamworks;
using Teamworks.Web.Models.Api;

namespace Teamworks.Web.Controllers.Api
{
    [DefaultHttpRouteConvention]
    [RoutePrefix("api/projects/{projectid}")]
    public class MessagesController : RavenApiController
    {
        #region Project

        private Core.Discussion LoadProjectDiscussion(int projectid, int discussionid)
        {
            var project = DbSession
                .Include<Core.Project>(p => p.Discussions)
                .Load<Core.Project>(projectid);
            if (project == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            var topic = DbSession.Load<Core.Discussion>(discussionid);
            if (topic == null || !project.Discussions.Contains(topic.Id))
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return topic;
        }
        
        [GET("discussions/{discussionid}/messages")]
        public IEnumerable<Message> GetProjectDiscussionMessage(int projectid, int discussionid)
        {
            return LoadProjectDiscussion(projectid, discussionid).Messages.Select(Mapper.Map<Core.Message, Message>);
        }

        [POST("discussions/{discussionid}/messages")]
        public HttpResponseMessage PostProjectDiscussionMessage(
            int discussionid,
            int projectid,
            Message model)
        {
            var discussion = LoadProjectDiscussion(projectid, discussionid);

            var userPrincipalId = Request.GetCurrentPersonId();
            var message = Core.Message.Forge(model.Content, userPrincipalId);
            message.Id = discussion.GenerateNewTimeEntryId();

            discussion.Messages.Add(message);
            discussion.Notify(message);

            var value = Mapper.Map<Core.Message, Message>(message);
            return Request.CreateResponse(HttpStatusCode.Created, value);
        }

        [DELETE("discussions/{discussionid}/messages/{id}")]
        public HttpResponseMessage DeleteProjectDiscussionMessage(int id, int projectid, int discussionid)
        {
            var discussion = LoadProjectDiscussion(projectid, discussionid);

            var message = discussion.Messages.FirstOrDefault(m => m.Id == id);
            if (message == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            discussion.Messages.Remove(message);
            return Request.CreateResponse(HttpStatusCode.NoContent);
        }

        #endregion

        #region Activity

        private Core.Discussion LoadTaskDiscussion(int projectid, int activityid, int discussionid)
        {
            var project = DbSession
                .Include<Core.Project>(p => p.Activities)
                .Load<Core.Project>(projectid);

            if (project == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            var task = DbSession
                .Include<Core.Activity>(t => t.Discussions)
                .Load<Core.Activity>(activityid);

            if (task == null || !task.Project.Equals(project.Id))
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            var discussion = DbSession.Load<Core.Discussion>(discussionid);
            if (discussion == null || !discussion.Entity.Equals(task.Id))
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

        #endregion
    }
}