using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using System.Web.Http.ModelBinding.Binders;
using AttributeRouting;
using AttributeRouting.Web.Http;
using AutoMapper;
using Teamworks.Core.Projects;
using Teamworks.Web.Helpers.Api;
using Teamworks.Web.Helpers.Extensions;
using Teamworks.Web.Models;
  //  [RoutePrefix("api/projects/{projectid}/discussions/{discussionid}/messages")]

namespace Teamworks.Web.Controllers.Api
{
    [DefaultHttpRouteConvention]
    [RoutePrefix("api/projects/{projectid}")]
    public class MessageController : RavenApiController
    {
        #region Project
        private Thread LoadProjectDiscussion(int projectid, int discussionid)
        {
            var project = DbSession
                .Include<Project>(p => p.Threads)
                .Load<Project>(projectid);
            if (project == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            var topic = DbSession.Load<Thread>(discussionid);
            if (topic == null || !project.Threads.Contains(topic.Id))
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            return topic;
        }

        [GET("discussions/{discussionid}/messages")]
        public IEnumerable<MessageModel> GetProjectDiscussionMessage(int projectid, int discussionid)
        {
            return LoadProjectDiscussion(projectid, discussionid).Messages.Select(Mapper.Map<Message, MessageModel>);
        }

        [POST("discussions/{discussionid}/messages")]
        public HttpResponseMessage<MessageModel> PostProjectDiscussionMessage([ModelBinder(typeof(TypeConverterModelBinder))] int discussionid,
                                                      [ModelBinder(typeof(TypeConverterModelBinder))] int projectid,
                                                      MessageModel model)
        {
            if (!ModelState.IsValid)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            var topic = LoadProjectDiscussion(projectid, discussionid);

            var message = Message.Forge(model.Text, Request.GetUserPrincipalId());
            message.Id = topic.GenerateNewTimeEntryId();
            topic.Messages.Add(message);

            return new HttpResponseMessage<MessageModel>(Mapper.Map<Message, MessageModel>(message),
                                                         HttpStatusCode.Created);
        }

        /// <see cref="http://forums.asp.net/post/4855634.aspx" />
        [PUT("discussions/{discussionid}/messages")]
        public HttpResponseMessage<MessageModel> PutProjectDiscussionMessage([ModelBinder(typeof(TypeConverterModelBinder))] int discussionid,
                                                     [ModelBinder(typeof(TypeConverterModelBinder))] int projectid,
                                                     MessageModel model)
        {
            throw new NotImplementedException();
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
    
        #region Task
        private Thread LoadTaskDiscussion(int projectid, int taskid, int discussionid)
        {
            var project = DbSession
                           .Include<Project>(p => p.Tasks)
                           .Load<Project>(projectid);

            if (project == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            var task = DbSession
                .Include<Task>(t => t.Threads)
                .Load<Task>(taskid);

            if (task == null || !task.Project.Equals(project.Id))
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            var topic = DbSession.Load<Thread>(discussionid);
            if (topic == null || !topic.Entity.Equals(task.Id))
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            return topic;
        }

        [GET("tasks/{taskid}/discussions/{discussionid}/messages")]
        public IEnumerable<MessageModel> GetTaskDiscussionMessage(int projectid, int taskid, int discussionid)
        {
            return LoadTaskDiscussion(projectid, taskid, discussionid).Messages.Select(Mapper.Map<Message, MessageModel>);
        }

        [POST("tasks/{taskid}/discussions/{discussionid}/messages")]
        public HttpResponseMessage<MessageModel> PostTaskDiscussionMessage([ModelBinder(typeof(TypeConverterModelBinder))] int discussionid,
                                                      [ModelBinder(typeof(TypeConverterModelBinder))] int projectid,
                                                      [ModelBinder(typeof(TypeConverterModelBinder))] int taskid,
                                                      MessageModel model)
        {
            if (!ModelState.IsValid)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            var topic = LoadTaskDiscussion(projectid, taskid, discussionid);

            var message = Message.Forge(model.Text, Request.GetUserPrincipalId());
            message.Id = topic.GenerateNewTimeEntryId();
            topic.Messages.Add(message);

            return new HttpResponseMessage<MessageModel>(Mapper.Map<Message, MessageModel>(message),
                                                         HttpStatusCode.Created);
        }

        /// <see cref="http://forums.asp.net/post/4855634.aspx" />
        [PUT("tasks/{taskid}/discussions/{discussionid}/messages")]
        public HttpResponseMessage<MessageModel> PutTaskDiscussionMessage([ModelBinder(typeof(TypeConverterModelBinder))] int discussionid,
                                                     [ModelBinder(typeof(TypeConverterModelBinder))] int projectid,
                                                     [ModelBinder(typeof(TypeConverterModelBinder))] int taskid,
                                                     MessageModel model)
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