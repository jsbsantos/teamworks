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
using Board = Teamworks.Web.Models.Board;
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
                .Include<Project>(p => p.Boards)
                .Load<Project>(projectid);

            if (project == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            return project;
        }

        [GET("discussions")]
        public IEnumerable<DryBoard> GetProjectDiscussion(int projectid)
        {
            return
                new List<DryBoard>(
                    DbSession.Load<Core.Board>(LoadProject(projectid).Boards).Select(Mapper.Map<Core.Board, DryBoard>));
        }

        [GET("discussions/{id}")]
        public Board GetProjectDiscussion(int id, int projectid)
        {
            var project = LoadProject(projectid);
            var topic = DbSession.Load<Core.Board>(id);
            if (topic == null || !topic.Entity.Equals(project.Id))
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return Mapper.Map<Core.Board, Board>(topic);
        }

        [POST("discussions")]
        public HttpResponseMessage<Board> PostProjectDiscussion(
            [ModelBinder(typeof (TypeConverterModelBinder))] int projectid,
            Board model)
        {
            if (!ModelState.IsValid)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            var project = LoadProject(projectid);

            Core.Board board = Core.Board.Forge(model.Name, model.Text, project.Id, Request.GetUserPrincipalId());
            DbSession.Store(board);
            project.Boards.Add(board.Id);

            return new HttpResponseMessage<Board>(Mapper.Map<Core.Board, Board>(board),
                                                       HttpStatusCode.Created);
        }

        /// <see cref="http://forums.asp.net/post/4855634.aspx" />
        [PUT("discussions/{id}")]
        public HttpResponseMessage PutProjectDiscussion([ModelBinder(typeof (TypeConverterModelBinder))] int id,
                                                        [ModelBinder(typeof (TypeConverterModelBinder))] int projectid,
                                                        Board model)
        {
            if (!ModelState.IsValid)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            var project = LoadProject(projectid);
            var topic = DbSession.Load<Core.Board>(id);
            if (topic == null || !topic.Entity.Equals(project.Id))
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            topic.Text = model.Text;

            return new HttpResponseMessage<Board>(Mapper.Map<Core.Board, Board>(topic),
                                                       HttpStatusCode.Created);
        }

        [DELETE("discussions/{id}")]
        public HttpResponseMessage DeleteProjectDiscussion(int id, int projectid)
        {
            var project = LoadProject(projectid);
            var topic = DbSession.Load<Core.Board>(id);
            if (topic == null || !topic.Entity.Equals(project.Id))
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            project.Boards.Remove(topic.Id);
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
        public IEnumerable<DryBoard> GetTaskDiscussion(int projectid, int taskid)
        {
            return
                new List<DryBoard>(
                    DbSession.Load<Core.Board>(LoadTask(projectid, taskid).Boards).Select(
                        Mapper.Map<Core.Board, DryBoard>));
        }

        [GET("tasks/{taskid}/discussions/{id}")]
        public Board GetTaskDiscussion(int id, int projectid, int taskid)
        {
            var task = LoadTask(projectid, taskid);
            var topic = DbSession.Load<Core.Board>(id);
            if (topic == null || !topic.Entity.Equals(task.Id))
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return Mapper.Map<Core.Board, Board>(topic);
        }

        [POST("tasks/{taskid}/discussions/")]
        public HttpResponseMessage<Board> PostTaskDiscussion(
            [ModelBinder(typeof (TypeConverterModelBinder))] int projectid,
            [ModelBinder(typeof (TypeConverterModelBinder))] int taskid,
            Board model)
        {
            if (!ModelState.IsValid)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            var task = LoadTask(projectid, taskid);

            Core.Board board = Core.Board.Forge(model.Name, model.Text, task.Id, Request.GetUserPrincipalId());
            DbSession.Store(board);
            task.Boards.Add(board.Id);

            return new HttpResponseMessage<Board>(Mapper.Map<Core.Board, Board>(board),
                                                       HttpStatusCode.Created);
        }

        /// <see cref="http://forums.asp.net/post/4855634.aspx" />
        [PUT("tasks/{taskid}/discussions/{id}")]
        public HttpResponseMessage PutTaskDiscussion([ModelBinder(typeof (TypeConverterModelBinder))] int id,
                                                     [ModelBinder(typeof (TypeConverterModelBinder))] int projectid,
                                                     [ModelBinder(typeof (TypeConverterModelBinder))] int taskid,
                                                     Reply model)
        {
            if (!ModelState.IsValid)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            var task = LoadTask(projectid, taskid);
            var topic = DbSession.Load<Core.Board>(id);
            if (topic == null || !topic.Entity.Equals(task.Id))
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            topic.Text = model.Text;

            return new HttpResponseMessage<Board>(Mapper.Map<Core.Board, Board>(topic),
                                                       HttpStatusCode.Created);
        }

        [DELETE("tasks/{taskid}/discussions/{id}")]
        public HttpResponseMessage DeleteTaskDiscussion(int id, int projectid, int taskid)
        {
            var task = LoadTask(projectid, taskid);
            var topic = DbSession.Load<Core.Board>(id);
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