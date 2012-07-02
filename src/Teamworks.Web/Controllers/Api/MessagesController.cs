using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using System.Web.Http.ModelBinding.Binders;
using AttributeRouting;
using AttributeRouting.Web.Http;
using AutoMapper;
using Teamworks.Web.Helpers.Api;
using Teamworks.Web.Helpers.Teamworks;
using Teamworks.Web.Models;

//  [RoutePrefix("api/projects/{projectid}/discussions/{discussionid}/messages")]
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
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            var topic = DbSession.Load<Core.Discussion>(discussionid);
            if (topic == null || !project.Discussions.Contains(topic.Id))
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            return topic;
        }

        [GET("discussions/{discussionid}/messages")]
        public IEnumerable<Message> GetProjectDiscussionMessage(int projectid, int discussionid)
        {
            return LoadProjectDiscussion(projectid, discussionid).Messages.Select(Mapper.Map<Core.Message, Message>);
        }

        [POST("discussions/{discussionid}/messages")]
        public HttpResponseMessage<Message> PostProjectDiscussionMessage(
            [ModelBinder(typeof (TypeConverterModelBinder))] int discussionid,
            [ModelBinder(typeof (TypeConverterModelBinder))] int projectid,
            Message model)
        {
            if (!ModelState.IsValid)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            var topic = LoadProjectDiscussion(projectid, discussionid);

            var userPrincipalId = Request.GetUserPrincipalId();
            var message = Core.Message.Forge(model.Content, userPrincipalId);
            message.Id = topic.GenerateNewTimeEntryId();
            topic.Messages.Add(message);

            topic.Notify(message);

            return new HttpResponseMessage<Message>(Mapper.Map<Core.Message, Message>(message),
                                                         HttpStatusCode.Created);
        }

        [DELETE("discussions/{discussionid}/messages/{id}")]
        public HttpResponseMessage DeleteProjectDiscussionMessage(int id, int projectid, int discussionid)
        {
            var topic = LoadProjectDiscussion(projectid, discussionid);

            var message = topic.Messages.FirstOrDefault(m => m.Id == id);
            if (message == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            topic.Messages.Remove(message);

            return new HttpResponseMessage(HttpStatusCode.NoContent);
        }
        #endregion

        #region Activity
        private Core.Discussion LoadTaskDiscussion(int projectid, int taskid, int discussionid)
        {
            var project = DbSession
                .Include<Core.Project>(p => p.Activities)
                .Load<Core.Project>(projectid);

            if (project == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            var task = DbSession
                .Include<Core.Activity>(t => t.Discussions)
                .Load<Core.Activity>(taskid);

            if (task == null || !task.Project.Equals(project.Id))
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            var discussion = DbSession.Load<Core.Discussion>(discussionid);
            if (discussion == null || !discussion.Entity.Equals(task.Id))
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return discussion;
        }

        [GET("tasks/{taskid}/discussions/{discussionid}/messages")]
        public IEnumerable<Message> GetTaskDiscussionMessage(int projectid, int taskid, int discussionid)
        {
            return LoadTaskDiscussion(projectid, taskid, discussionid).Messages.Select(Mapper.Map<Core.Message, Message>);
        }

        [POST("tasks/{taskid}/discussions/{discussionid}/messages")]
        public HttpResponseMessage<Message> PostTaskDiscussionMessage(
            [ModelBinder(typeof (TypeConverterModelBinder))] int discussionid,
            [ModelBinder(typeof (TypeConverterModelBinder))] int projectid,
            [ModelBinder(typeof (TypeConverterModelBinder))] int taskid,
            Message model)
        {
            var topic = LoadTaskDiscussion(projectid, taskid, discussionid);

            var message = Core.Message.Forge(model.Content, Request.GetUserPrincipalId());
            message.Id = topic.GenerateNewTimeEntryId();
            topic.Messages.Add(message);

            return new HttpResponseMessage<Message>(Mapper.Map<Core.Message, Message>(message),
                                                         HttpStatusCode.Created);
        }

        /// <see cref="http://forums.asp.net/post/4855634.aspx" />
        [PUT("tasks/{taskid}/discussions/{discussionid}/messages")]
        public HttpResponseMessage<Message> PutTaskDiscussionMessage(
            [ModelBinder(typeof (TypeConverterModelBinder))] int discussionid,
            [ModelBinder(typeof (TypeConverterModelBinder))] int projectid,
            [ModelBinder(typeof (TypeConverterModelBinder))] int taskid,
            Message model)
        {
            throw new NotImplementedException();
        }

        [DELETE("tasks/{taskid}/discussions/{discussionid}/messages/{id}")]
        public HttpResponseMessage DeleteTaskDiscussionMessage(int id, int projectid, int taskid, int discussionid)
        {
            var topic = LoadTaskDiscussion(projectid, taskid, discussionid);

            var message = topic.Messages.FirstOrDefault(m => m.Id == id);
            if (message == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            topic.Messages.Remove(message);

            return new HttpResponseMessage(HttpStatusCode.NoContent);
        }

        #endregion
    }
}