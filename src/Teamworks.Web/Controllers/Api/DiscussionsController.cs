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
using Teamworks.Web.Models;
using Teamworks.Web.Models.DryModels;

namespace Teamworks.Web.Controllers.Api
{
    [RoutePrefix("api/projects/{projectid}")]
    public class DiscussionsController : RavenApiController
    {
        #region Entity Threads

        private Project LoadProject(int projectid)
        {
            var project = DbSession
                .Include<Project>(p => p.Threads)
                .Load<Project>(projectid);

            if (project == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            return project;
        }

        [GET("discussions")]
        public IEnumerable<DryThreadModel> GetProjectDiscussion(int projectid)
        {
            return
                new List<DryThreadModel>(
                    DbSession.Load<Thread>(LoadProject(projectid).Threads).Select(Mapper.Map<Thread, DryThreadModel>));
        }

        [GET("discussions/{id}")]
        public ThreadModel GetProjectDiscussion(int id, int projectid)
        {
            var project = LoadProject(projectid);
            var topic = DbSession.Load<Thread>(id);
            if (topic == null || !topic.Entity.Equals(project.Id))
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return Mapper.Map<Thread, ThreadModel>(topic);
        }

        [POST("discussions")]
        public HttpResponseMessage<ThreadModel> PostProjectDiscussion(
            [ModelBinder(typeof (TypeConverterModelBinder))] int projectid,
            ThreadModel model)
        {
            if (!ModelState.IsValid)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            var project = LoadProject(projectid);

            Thread thread = Thread.Forge(model.Name, model.Text, project.Id, Request.GetUserPrincipalId());
            DbSession.Store(thread);
            project.Threads.Add(thread.Id);
            DbSession.SaveChanges();

            return new HttpResponseMessage<ThreadModel>(Mapper.Map<Thread, ThreadModel>(thread),
                                                       HttpStatusCode.Created);
        }

        /// <see cref="http://forums.asp.net/post/4855634.aspx" />
        [PUT("discussions/{id}")]
        public HttpResponseMessage PutProjectDiscussion([ModelBinder(typeof (TypeConverterModelBinder))] int id,
                                                        [ModelBinder(typeof (TypeConverterModelBinder))] int projectid,
                                                        ThreadModel model)
        {
            if (!ModelState.IsValid)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            var project = LoadProject(projectid);
            var topic = DbSession.Load<Thread>(id);
            if (topic == null || !topic.Entity.Equals(project.Id))
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            topic.Text = model.Text;
            DbSession.SaveChanges();

            return new HttpResponseMessage<ThreadModel>(Mapper.Map<Thread, ThreadModel>(topic),
                                                       HttpStatusCode.Created);
        }

        [DELETE("discussions/{id}")]
        public HttpResponseMessage DeleteProjectDiscussion(int id, int projectid)
        {
            var project = LoadProject(projectid);
            var topic = DbSession.Load<Thread>(id);
            if (topic == null || !topic.Entity.Equals(project.Id))
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            project.Threads.Remove(topic.Id);
            DbSession.Delete(topic);
            DbSession.SaveChanges();

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
                .Include<Task>(t => t.Threads)
                .Load<Task>(taskid);

            if (task == null || !task.Project.Equals(project.Id))
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return task;
        }

        [GET("tasks/{taskid}/discussions")]
        public IEnumerable<DryThreadModel> GetTaskDiscussion(int projectid, int taskid)
        {
            return
                new List<DryThreadModel>(
                    DbSession.Load<Thread>(LoadTask(projectid, taskid).Threads).Select(
                        Mapper.Map<Thread, DryThreadModel>));
        }

        [GET("tasks/{taskid}/discussions/{id}")]
        public ThreadModel GetTaskDiscussion(int id, int projectid, int taskid)
        {
            var task = LoadTask(projectid, taskid);
            var topic = DbSession.Load<Thread>(id);
            if (topic == null || !topic.Entity.Equals(task.Id))
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return Mapper.Map<Thread, ThreadModel>(topic);
        }

        [POST("tasks/{taskid}/discussions/")]
        public HttpResponseMessage<ThreadModel> PostTaskDiscussion(
            [ModelBinder(typeof (TypeConverterModelBinder))] int projectid,
            [ModelBinder(typeof (TypeConverterModelBinder))] int taskid,
            ThreadModel model)
        {
            if (!ModelState.IsValid)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            var task = LoadTask(projectid, taskid);

            Thread thread = Thread.Forge(model.Name, model.Text, task.Id, Request.GetUserPrincipalId());
            DbSession.Store(thread);
            task.Threads.Add(thread.Id);
            DbSession.SaveChanges();

            return new HttpResponseMessage<ThreadModel>(Mapper.Map<Thread, ThreadModel>(thread),
                                                       HttpStatusCode.Created);
        }

        /// <see cref="http://forums.asp.net/post/4855634.aspx" />
        [PUT("tasks/{taskid}/discussions/{id}")]
        public HttpResponseMessage PutTaskDiscussion([ModelBinder(typeof (TypeConverterModelBinder))] int id,
                                                     [ModelBinder(typeof (TypeConverterModelBinder))] int projectid,
                                                     [ModelBinder(typeof (TypeConverterModelBinder))] int taskid,
                                                     MessageModel model)
        {
            if (!ModelState.IsValid)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            var task = LoadTask(projectid, taskid);
            var topic = DbSession.Load<Thread>(id);
            if (topic == null || !topic.Entity.Equals(task.Id))
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            topic.Text = model.Text;
            DbSession.SaveChanges();

            return new HttpResponseMessage<ThreadModel>(Mapper.Map<Thread, ThreadModel>(topic),
                                                       HttpStatusCode.Created);
        }

        [DELETE("tasks/{taskid}/discussions/{id}")]
        public HttpResponseMessage DeleteTaskDiscussion(int id, int projectid, int taskid)
        {
            var task = LoadTask(projectid, taskid);
            var topic = DbSession.Load<Thread>(id);
            if (topic == null || !topic.Entity.Equals(task.Id))
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            task.Threads.Remove(topic.Id);
            DbSession.Delete(topic);
            DbSession.SaveChanges();

            return new HttpResponseMessage(HttpStatusCode.NoContent);
        }

        #endregion Task
    }
}