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
using Teamworks.Web.Models.DryModels;
using Activity = Teamworks.Core.Activity;
using Discussion = Teamworks.Web.Models.Discussion;
using Message = Teamworks.Web.Models.Message;
using Project = Teamworks.Core.Project;

namespace Teamworks.Web.Controllers.Api
{
    [RoutePrefix("api/projects/{projectid}")]
    public class DiscussionsController : RavenApiController
    {
        #region Project Discussion

        private Project LoadProject(int projectid)
        {
            var project = DbSession
                .Include<Project>(p => p.Discussions)
                .Load<Project>(projectid);

            if (project == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            return project;
        }

        [GET("discussions")]
        public IEnumerable<DryDiscussion> GetProjectDiscussion(int projectid)
        {
            return
                new List<DryDiscussion>(
                    DbSession.Load<Core.Discussion>(LoadProject(projectid).Discussions).Select(Mapper.Map<Core.Discussion, DryDiscussion>));
        }

        [GET("discussions/{id}")]
        public Discussion GetProjectDiscussion(int id, int projectid)
        {
            var project = LoadProject(projectid);
            var topic = DbSession.Load<Core.Discussion>(id);
            if (topic == null || !topic.Entity.Equals(project.Id))
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return Mapper.Map<Core.Discussion, Discussion>(topic);
        }

        [POST("discussions")]
        public HttpResponseMessage<Discussion> PostProjectDiscussion(
            [ModelBinder(typeof (TypeConverterModelBinder))] int projectid,
            Discussion model)
        {
            if (!ModelState.IsValid)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            var project = LoadProject(projectid);

            Core.Discussion discussion = Core.Discussion.Forge(model.Name, model.Content, project.Id, Request.GetUserPrincipalId());
            DbSession.Store(discussion);
            project.Discussions.Add(discussion.Id);

            return new HttpResponseMessage<Discussion>(Mapper.Map<Core.Discussion, Discussion>(discussion),
                                                       HttpStatusCode.Created);
        }

        /// <see cref="http://forums.asp.net/post/4855634.aspx" />
        [PUT("discussions/{id}")]
        public HttpResponseMessage PutProjectDiscussion([ModelBinder(typeof (TypeConverterModelBinder))] int id,
                                                        [ModelBinder(typeof (TypeConverterModelBinder))] int projectid,
                                                        Discussion model)
        {
            if (!ModelState.IsValid)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            var project = LoadProject(projectid);
            var topic = DbSession.Load<Core.Discussion>(id);
            if (topic == null || !topic.Entity.Equals(project.Id))
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            topic.Content = model.Content;

            return new HttpResponseMessage<Discussion>(Mapper.Map<Core.Discussion, Discussion>(topic),
                                                       HttpStatusCode.Created);
        }

        [DELETE("discussions/{id}")]
        public HttpResponseMessage DeleteProjectDiscussion(int id, int projectid)
        {
            var project = LoadProject(projectid);
            var topic = DbSession.Load<Core.Discussion>(id);
            if (topic == null || !topic.Entity.Equals(project.Id))
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            project.Discussions.Remove(topic.Id);
            DbSession.Delete(topic);

            return new HttpResponseMessage(HttpStatusCode.NoContent);
        }

        #endregion

        #region Activities Discussion

        private Activity LoadTask(int projectid, int taskid)
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
            return task;
        }

        [GET("tasks/{taskid}/discussions")]
        public IEnumerable<DryDiscussion> GetTaskDiscussion(int projectid, int taskid)
        {
            return
                new List<DryDiscussion>(
                    DbSession.Load<Core.Discussion>(LoadTask(projectid, taskid).Discussions).Select(
                        Mapper.Map<Core.Discussion, DryDiscussion>));
        }

        [GET("tasks/{taskid}/discussions/{id}")]
        public Discussion GetTaskDiscussion(int id, int projectid, int taskid)
        {
            var task = LoadTask(projectid, taskid);
            var topic = DbSession.Load<Core.Discussion>(id);
            if (topic == null || !topic.Entity.Equals(task.Id))
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return Mapper.Map<Core.Discussion, Discussion>(topic);
        }

        [POST("tasks/{taskid}/discussions/")]
        public HttpResponseMessage<Discussion> PostTaskDiscussion(
            [ModelBinder(typeof (TypeConverterModelBinder))] int projectid,
            [ModelBinder(typeof (TypeConverterModelBinder))] int taskid,
            Discussion model)
        {
            if (!ModelState.IsValid)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            var task = LoadTask(projectid, taskid);

            Core.Discussion discussion = Core.Discussion.Forge(model.Name, model.Content, task.Id, Request.GetUserPrincipalId());
            DbSession.Store(discussion);
            task.Discussions.Add(discussion.Id);

            return new HttpResponseMessage<Discussion>(Mapper.Map<Core.Discussion, Discussion>(discussion),
                                                       HttpStatusCode.Created);
        }

        /// <see cref="http://forums.asp.net/post/4855634.aspx" />
        [PUT("tasks/{taskid}/discussions/{id}")]
        public HttpResponseMessage PutTaskDiscussion([ModelBinder(typeof (TypeConverterModelBinder))] int id,
                                                     [ModelBinder(typeof (TypeConverterModelBinder))] int projectid,
                                                     [ModelBinder(typeof (TypeConverterModelBinder))] int taskid,
                                                     Message model)
        {
            if (!ModelState.IsValid)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            var task = LoadTask(projectid, taskid);
            var topic = DbSession.Load<Core.Discussion>(id);
            if (topic == null || !topic.Entity.Equals(task.Id))
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            topic.Content = model.Content;

            return new HttpResponseMessage<Discussion>(Mapper.Map<Core.Discussion, Discussion>(topic),
                                                       HttpStatusCode.Created);
        }

        [DELETE("tasks/{taskid}/discussions/{id}")]
        public HttpResponseMessage DeleteTaskDiscussion(int id, int projectid, int taskid)
        {
            var task = LoadTask(projectid, taskid);
            var topic = DbSession.Load<Core.Discussion>(id);
            if (topic == null || !topic.Entity.Equals(task.Id))
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            task.Discussions.Remove(topic.Id);
            DbSession.Delete(topic);

            return new HttpResponseMessage(HttpStatusCode.NoContent);
        }

        #endregion Activities
    }
}