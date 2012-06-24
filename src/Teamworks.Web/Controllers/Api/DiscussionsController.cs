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
using Message = Teamworks.Web.Models.Message;
using Project = Teamworks.Core.Project;
using Task = Teamworks.Core.Task;

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
        public IEnumerable<DryDiscussions> GetProjectDiscussion(int projectid)
        {
            return
                new List<DryDiscussions>(
                    DbSession.Load<Core.Discussion>(LoadProject(projectid).Discussions).Select(Mapper.Map<Core.Discussion, DryDiscussions>));
        }

        [GET("discussions/{id}")]
        public Discussions GetProjectDiscussion(int id, int projectid)
        {
            var project = LoadProject(projectid);
            var topic = DbSession.Load<Core.Discussion>(id);
            if (topic == null || !topic.Entity.Equals(project.Id))
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return Mapper.Map<Core.Discussion, Discussions>(topic);
        }

        [POST("discussions")]
        public HttpResponseMessage<Discussions> PostProjectDiscussion(
            [ModelBinder(typeof (TypeConverterModelBinder))] int projectid,
            Discussions model)
        {
            if (!ModelState.IsValid)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            var project = LoadProject(projectid);

            Core.Discussion discussion = Core.Discussion.Forge(model.Name, model.Content, project.Id, Request.GetUserPrincipalId());
            DbSession.Store(discussion);
            project.Discussions.Add(discussion.Id);

            return new HttpResponseMessage<Discussions>(Mapper.Map<Core.Discussion, Discussions>(discussion),
                                                       HttpStatusCode.Created);
        }

        /// <see cref="http://forums.asp.net/post/4855634.aspx" />
        [PUT("discussions/{id}")]
        public HttpResponseMessage PutProjectDiscussion([ModelBinder(typeof (TypeConverterModelBinder))] int id,
                                                        [ModelBinder(typeof (TypeConverterModelBinder))] int projectid,
                                                        Discussions model)
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

            return new HttpResponseMessage<Discussions>(Mapper.Map<Core.Discussion, Discussions>(topic),
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

        #region Task Discussion

        private Task LoadTask(int projectid, int taskid)
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
            return task;
        }

        [GET("tasks/{taskid}/discussions")]
        public IEnumerable<DryDiscussions> GetTaskDiscussion(int projectid, int taskid)
        {
            return
                new List<DryDiscussions>(
                    DbSession.Load<Core.Discussion>(LoadTask(projectid, taskid).Boards).Select(
                        Mapper.Map<Core.Discussion, DryDiscussions>));
        }

        [GET("tasks/{taskid}/discussions/{id}")]
        public Discussions GetTaskDiscussion(int id, int projectid, int taskid)
        {
            var task = LoadTask(projectid, taskid);
            var topic = DbSession.Load<Core.Discussion>(id);
            if (topic == null || !topic.Entity.Equals(task.Id))
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return Mapper.Map<Core.Discussion, Discussions>(topic);
        }

        [POST("tasks/{taskid}/discussions/")]
        public HttpResponseMessage<Discussions> PostTaskDiscussion(
            [ModelBinder(typeof (TypeConverterModelBinder))] int projectid,
            [ModelBinder(typeof (TypeConverterModelBinder))] int taskid,
            Discussions model)
        {
            if (!ModelState.IsValid)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            var task = LoadTask(projectid, taskid);

            Core.Discussion discussion = Core.Discussion.Forge(model.Name, model.Content, task.Id, Request.GetUserPrincipalId());
            DbSession.Store(discussion);
            task.Boards.Add(discussion.Id);

            return new HttpResponseMessage<Discussions>(Mapper.Map<Core.Discussion, Discussions>(discussion),
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

            topic.Content = model.Text;

            return new HttpResponseMessage<Discussions>(Mapper.Map<Core.Discussion, Discussions>(topic),
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

            task.Boards.Remove(topic.Id);
            DbSession.Delete(topic);

            return new HttpResponseMessage(HttpStatusCode.NoContent);
        }

        #endregion Task
    }
}