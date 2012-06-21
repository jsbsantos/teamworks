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
using Board = Teamworks.Core.Board;
using Project = Teamworks.Core.Project;
using Task = Teamworks.Core.Task;

//  [RoutePrefix("api/projects/{projectid}/discussions/{discussionid}/messages")]

namespace Teamworks.Web.Controllers.Api
{
    [DefaultHttpRouteConvention]
    [RoutePrefix("api/projects/{projectid}")]
    public class MessageController : RavenApiController
    {
        #region Project
        private Board LoadProjectDiscussion(int projectid, int discussionid)
        {
            var project = DbSession
                .Include<Project>(p => p.Boards)
                .Load<Project>(projectid);
            if (project == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            var topic = DbSession.Load<Board>(discussionid);
            if (topic == null || !project.Boards.Contains(topic.Id))
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            return topic;
        }

        [GET("discussions/{discussionid}/messages")]
        public IEnumerable<Reply> GetProjectDiscussionMessage(int projectid, int discussionid)
        {
            return LoadProjectDiscussion(projectid, discussionid).Messages.Select(Mapper.Map<Core.Message, Reply>);
        }

        [POST("discussions/{discussionid}/messages")]
        public HttpResponseMessage<Reply> PostProjectDiscussionMessage([ModelBinder(typeof(TypeConverterModelBinder))] int discussionid,
                                                      [ModelBinder(typeof(TypeConverterModelBinder))] int projectid,
                                                      Reply model)
        {
            if (!ModelState.IsValid)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            var topic = LoadProjectDiscussion(projectid, discussionid);

            var message = Core.Message.Forge(model.Text, Request.GetUserPrincipalId());
            message.Id = topic.GenerateNewTimeEntryId();
            topic.Messages.Add(message);

            return new HttpResponseMessage<Reply>(Mapper.Map<Core.Message, Reply>(message),
                                                         HttpStatusCode.Created);
        }

        /// <see cref="http://forums.asp.net/post/4855634.aspx" />
        [PUT("discussions/{discussionid}/messages")]
        public HttpResponseMessage<Reply> PutProjectDiscussionMessage([ModelBinder(typeof(TypeConverterModelBinder))] int discussionid,
                                                     [ModelBinder(typeof(TypeConverterModelBinder))] int projectid,
                                                     Reply model)
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
        private Board LoadTaskDiscussion(int projectid, int taskid, int discussionid)
        {
            var project = DbSession
                           .Include<Project>(p => p.Tasks)
                           .Load<Project>(projectid);

            if (project == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            var task = DbSession
                .Include<Task>(t => t.Boards)
                .Load<Task>(taskid);

            if (task == null || !task.Project.Equals(project.Id))
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            var topic = DbSession.Load<Board>(discussionid);
            if (topic == null || !topic.Entity.Equals(task.Id))
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            return topic;
        }

        [GET("tasks/{taskid}/discussions/{discussionid}/messages")]
        public IEnumerable<Reply> GetTaskDiscussionMessage(int projectid, int taskid, int discussionid)
        {
            return LoadTaskDiscussion(projectid, taskid, discussionid).Messages.Select(Mapper.Map<Core.Message, Reply>);
        }

        [POST("tasks/{taskid}/discussions/{discussionid}/messages")]
        public HttpResponseMessage<Reply> PostTaskDiscussionMessage([ModelBinder(typeof(TypeConverterModelBinder))] int discussionid,
                                                      [ModelBinder(typeof(TypeConverterModelBinder))] int projectid,
                                                      [ModelBinder(typeof(TypeConverterModelBinder))] int taskid,
                                                      Reply model)
        {
            if (!ModelState.IsValid)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            var topic = LoadTaskDiscussion(projectid, taskid, discussionid);

            var message = Core.Message.Forge(model.Text, Request.GetUserPrincipalId());
            message.Id = topic.GenerateNewTimeEntryId();
            topic.Messages.Add(message);

            return new HttpResponseMessage<Reply>(Mapper.Map<Core.Message, Reply>(message),
                                                         HttpStatusCode.Created);
        }

        /// <see cref="http://forums.asp.net/post/4855634.aspx" />
        [PUT("tasks/{taskid}/discussions/{discussionid}/messages")]
        public HttpResponseMessage<Reply> PutTaskDiscussionMessage([ModelBinder(typeof(TypeConverterModelBinder))] int discussionid,
                                                     [ModelBinder(typeof(TypeConverterModelBinder))] int projectid,
                                                     [ModelBinder(typeof(TypeConverterModelBinder))] int taskid,
                                                     Reply model)
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