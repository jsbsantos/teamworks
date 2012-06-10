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
        #region Entity Discussions

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
        public IEnumerable<DryTopicModel> GetProjectDiscussion(int projectid)
        {
            return
                new List<DryTopicModel>(
                    DbSession.Load<Topic>(LoadProject(projectid).Discussions).Select(Mapper.Map<Topic, DryTopicModel>));
        }

        [GET("discussions/{id}")]
        public TopicModel GetProjectDiscussion(int id, int projectid)
        {
            var project = LoadProject(projectid);
            var topic = DbSession.Load<Topic>(id);
            if (topic == null || !topic.Entity.Equals(project.Id))
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return Mapper.Map<Topic, TopicModel>(topic);
        }

        [POST("discussions")]
        public HttpResponseMessage<TopicModel> PostProjectDiscussion(
            [ModelBinder(typeof (TypeConverterModelBinder))] int projectid,
            TopicModel model)
        {
            if (!ModelState.IsValid)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            var project = LoadProject(projectid);

            Topic topic = Topic.Forge(model.Name, model.Text, project.Id, Request.GetUserPrincipalId());
            DbSession.Store(topic);
            project.Discussions.Add(topic.Id);
            DbSession.SaveChanges();

            return new HttpResponseMessage<TopicModel>(Mapper.Map<Topic, TopicModel>(topic),
                                                       HttpStatusCode.Created);
        }

        /// <see cref="http://forums.asp.net/post/4855634.aspx" />
        [PUT("discussions/{id}")]
        public HttpResponseMessage PutProjectDiscussion([ModelBinder(typeof (TypeConverterModelBinder))] int id,
                                                        [ModelBinder(typeof (TypeConverterModelBinder))] int projectid,
                                                        TopicModel model)
        {
            if (!ModelState.IsValid)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            var project = LoadProject(projectid);
            var topic = DbSession.Load<Topic>(id);
            if (topic == null || !topic.Entity.Equals(project.Id))
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            topic.Text = model.Text;
            DbSession.SaveChanges();

            return new HttpResponseMessage<TopicModel>(Mapper.Map<Topic, TopicModel>(topic),
                                                       HttpStatusCode.Created);
        }

        [DELETE("discussions/{id}")]
        public HttpResponseMessage DeleteProjectDiscussion(int id, int projectid)
        {
            var project = LoadProject(projectid);
            var topic = DbSession.Load<Topic>(id);
            if (topic == null || !topic.Entity.Equals(project.Id))
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            project.Discussions.Remove(topic.Id);
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
                .Include<Task>(t => t.Discussions)
                .Load<Task>(taskid);

            if (task == null || !task.Project.Equals(project.Id))
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return task;
        }

        [GET("tasks/{taskid}/discussions")]
        public IEnumerable<DryTopicModel> GetTaskDiscussion(int projectid, int taskid)
        {
            return
                new List<DryTopicModel>(
                    DbSession.Load<Topic>(LoadTask(projectid, taskid).Discussions).Select(
                        Mapper.Map<Topic, DryTopicModel>));
        }

        [GET("tasks/{taskid}/discussions/{id}")]
        public TopicModel GetTaskDiscussion(int id, int projectid, int taskid)
        {
            var task = LoadTask(projectid, taskid);
            var topic = DbSession.Load<Topic>(id);
            if (topic == null || !topic.Entity.Equals(task.Id))
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return Mapper.Map<Topic, TopicModel>(topic);
        }

        [POST("tasks/{taskid}/discussions/")]
        public HttpResponseMessage<TopicModel> PostTaskDiscussion(
            [ModelBinder(typeof (TypeConverterModelBinder))] int projectid,
            [ModelBinder(typeof (TypeConverterModelBinder))] int taskid,
            TopicModel model)
        {
            if (!ModelState.IsValid)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            var task = LoadTask(projectid, taskid);

            Topic topic = Topic.Forge(model.Name, model.Text, task.Id, Request.GetUserPrincipalId());
            DbSession.Store(topic);
            task.Discussions.Add(topic.Id);
            DbSession.SaveChanges();

            return new HttpResponseMessage<TopicModel>(Mapper.Map<Topic, TopicModel>(topic),
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
            var topic = DbSession.Load<Topic>(id);
            if (topic == null || !topic.Entity.Equals(task.Id))
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            topic.Text = model.Text;
            DbSession.SaveChanges();

            return new HttpResponseMessage<TopicModel>(Mapper.Map<Topic, TopicModel>(topic),
                                                       HttpStatusCode.Created);
        }

        [DELETE("tasks/{taskid}/discussions/{id}")]
        public HttpResponseMessage DeleteTaskDiscussion(int id, int projectid, int taskid)
        {
            var task = LoadTask(projectid, taskid);
            var topic = DbSession.Load<Topic>(id);
            if (topic == null || !topic.Entity.Equals(task.Id))
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            task.Discussions.Remove(topic.Id);
            DbSession.Delete(topic);
            DbSession.SaveChanges();

            return new HttpResponseMessage(HttpStatusCode.NoContent);
        }

        #endregion Task
    }
}