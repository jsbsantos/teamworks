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
using Teamworks.Core;
using Teamworks.Web.Helpers.Api;
using Teamworks.Web.Models;
using Activity = Teamworks.Core.Activity;
using Discussion = Teamworks.Core.Discussion;
using Message = Teamworks.Web.Models.Message;
using Project = Teamworks.Core.Project;

//  [RoutePrefix("api/projects/{projectid}/discussions/{discussionid}/messages")]

namespace Teamworks.Web.Controllers.Api
{
    [DefaultHttpRouteConvention]
    [RoutePrefix("api/projects/{projectid}")]
    public class MessageController : RavenApiController
    {
        #region Project
        private Discussion LoadProjectDiscussion(int projectid, int discussionid)
        {
            var project = DbSession
                .Include<Project>(p => p.Discussions)
                .Load<Project>(projectid);
            if (project == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            var topic = DbSession.Load<Discussion>(discussionid);
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
        public HttpResponseMessage<Message> PostProjectDiscussionMessage([ModelBinder(typeof(TypeConverterModelBinder))] int discussionid,
                                                      [ModelBinder(typeof(TypeConverterModelBinder))] int projectid,
                                                      Message model)
        {
            if (!ModelState.IsValid)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            var topic = LoadProjectDiscussion(projectid, discussionid);

            var message = Core.Message.Forge(model.Content, Request.GetUserPrincipalId());
            message.Id = topic.GenerateNewTimeEntryId();
            topic.Messages.Add(message);

            return new HttpResponseMessage<Message>(Mapper.Map<Core.Message, Message>(message),
                                                         HttpStatusCode.Created);
        }

        /// <see cref="http://forums.asp.net/post/4855634.aspx" />
        [PUT("discussions/{discussionid}/messages")]
        public HttpResponseMessage<Message> PutProjectDiscussionMessage([ModelBinder(typeof(TypeConverterModelBinder))] int discussionid,
                                                     [ModelBinder(typeof(TypeConverterModelBinder))] int projectid,
                                                     Message model)
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
    
        #region Activities
        private Discussion LoadTaskDiscussion(int projectid, int taskid, int discussionid)
        {
            var project = DbSession
                           .Include<Project>(p => p.Activities)
                           .Load<Project>(projectid);

            if (project == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            var task = DbSession
                .Include<Activity>(t => t.Discussions)
                .Load<Activity>(taskid);

            if (task == null || !task.Project.Equals(project.Id))
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            var topic = DbSession.Load<Discussion>(discussionid);
            if (topic == null || !topic.Entity.Equals(task.Id))
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            return topic;
        }

        [GET("tasks/{taskid}/discussions/{discussionid}/messages")]
        public IEnumerable<Message> GetTaskDiscussionMessage(int projectid, int taskid, int discussionid)
        {
            return LoadTaskDiscussion(projectid, taskid, discussionid).Messages.Select(Mapper.Map<Core.Message, Message>);
        }

        [POST("tasks/{taskid}/discussions/{discussionid}/messages")]
        public HttpResponseMessage<Message> PostTaskDiscussionMessage([ModelBinder(typeof(TypeConverterModelBinder))] int discussionid,
                                                      [ModelBinder(typeof(TypeConverterModelBinder))] int projectid,
                                                      [ModelBinder(typeof(TypeConverterModelBinder))] int taskid,
                                                      Message model)
        {
            if (!ModelState.IsValid)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            var topic = LoadTaskDiscussion(projectid, taskid, discussionid);

            var message = Core.Message.Forge(model.Content, Request.GetUserPrincipalId());
            message.Id = topic.GenerateNewTimeEntryId();
            topic.Messages.Add(message);

            return new HttpResponseMessage<Message>(Mapper.Map<Core.Message, Message>(message),
                                                         HttpStatusCode.Created);
        }

        /// <see cref="http://forums.asp.net/post/4855634.aspx" />
        [PUT("tasks/{taskid}/discussions/{discussionid}/messages")]
        public HttpResponseMessage<Message> PutTaskDiscussionMessage([ModelBinder(typeof(TypeConverterModelBinder))] int discussionid,
                                                     [ModelBinder(typeof(TypeConverterModelBinder))] int projectid,
                                                     [ModelBinder(typeof(TypeConverterModelBinder))] int taskid,
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