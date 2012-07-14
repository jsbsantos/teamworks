using System.Linq;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using AttributeRouting;
using AttributeRouting.Web.Http;
using AutoMapper;
using Teamworks.Web.Controllers.Api.Attribute;
using Teamworks.Web.Helpers.Api;
using Teamworks.Web.Models.Api;

namespace Teamworks.Web.Controllers.Api
{
    [SecureFor("/projects")]
    [DefaultHttpRouteConvention]
    [RoutePrefix("api/projects/{projectid}/activities/{activityid}/todolist/{listid}/todo")]
    public class TodoController : RavenApiController
    {
        public IEnumerable<Todo> Get(int id, int projectid, int activityid)
        {
            var project = DbSession
                            .Include<Core.Project>(p => p.Activities)
                            .Load<Core.Project>(Request.GetCurrentPersonId());

            if (project.Activities.Contains(activityid.ToString()))
            {
                Request.NotFound();
                return null;
            }
            var activity = DbSession.Load<Core.Activity>(activityid);

            var list = activity.Todos.SingleOrDefault(p => p.Id == id);
            if (list != null)
                return Mapper.Map<IEnumerable<Core.Todo>, IEnumerable<Todo>>(list.Todos);

            Request.NotFound();
            return null;
        }

        public Todo Get(int id, int projectid, int activityid, int listid)
        {
            var project = DbSession
                            .Include<Core.Project>(p => p.Activities)
                            .Load<Core.Project>(Request.GetCurrentPersonId());

            if (project.Activities.Contains(activityid.ToString()))
            {
                Request.NotFound();
                return null;
            }
            var activity = DbSession.Load<Core.Activity>(activityid);

            var list = activity.Todos.Single(p => p.Id == listid);
            return Mapper.Map<Core.Todo, Todo>(list.Todos.Single(t => t.Id == id));
        }

        public HttpResponseMessage Post(int projectid, int activityid, int listid, Todo model)
        {
            var project = DbSession
                            .Include<Core.Project>(p => p.Activities)
                            .Load<Core.Project>(Request.GetCurrentPersonId());

            if (project.Activities.Contains(activityid.ToString()))
            {
                Request.NotFound();
                return null;
            }
            var activity = DbSession.Load<Core.Activity>(activityid);

            var list = activity.Todos.Single(p => p.Id == listid);

            var todo = Core.Todo.Forge(model.Name, model.Description, model.DueDate);
            todo.Id = list.GenerateNewTodoId();
            list.Todos.Add(todo);
            DbSession.SaveChanges();
            return Request.CreateResponse(HttpStatusCode.Created, Mapper.Map<Core.TodoList, TodoList>(list));
        }
        
        //Não reconhece o PUT sem attr; porquê?
        [PUT("")]
        public HttpResponseMessage Put(int projectid, int activityid, int listid, Todo model)
        {
            var project = DbSession
                            .Include<Core.Project>(p => p.Activities)
                            .Load<Core.Project>(Request.GetCurrentPersonId());

            if (project.Activities.Contains(activityid.ToString()))
            {
                Request.NotFound();
                return null;
            }
            var activity = DbSession.Load<Core.Activity>(activityid);
            var list = activity.Todos.Single(p => p.Id == listid);
            var todo = list.Todos.Single(t => t.Id == model.Id);

            todo.Name = model.Name;
            todo.Description = model.Description;
            todo.DueDate = model.DueDate;
            todo.Completed = model.Completed;
            DbSession.SaveChanges();

            return Request.CreateResponse(HttpStatusCode.Created, Mapper.Map<Core.TodoList, TodoList>(list));
        }

        public HttpResponseMessage Delete(int id, int projectid, int activityid, int listid)
        {
            var project = DbSession
                                        .Include<Core.Project>(p => p.Activities)
                                        .Load<Core.Project>(Request.GetCurrentPersonId());

            if (project.Activities.Contains(activityid.ToString()))
            {
                Request.NotFound();
                return null;
            }
            var activity = DbSession.Load<Core.Activity>(activityid);
            var list = activity.Todos.Single(p => p.Id == listid);
            var todo = list.Todos.Single(t => t.Id == id);

            list.Todos.Remove(todo);
            DbSession.SaveChanges();
            return new HttpResponseMessage(HttpStatusCode.NoContent);
        }
    }
}