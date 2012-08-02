using System.Collections;
using System.Linq;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using AttributeRouting;
using AttributeRouting.Web.Http;
using AutoMapper;
using Teamworks.Web.Attributes.Api;
using Teamworks.Web.Helpers.Api;
using TodoList = Teamworks.Web.Models.Api.TodoList;

namespace Teamworks.Web.Controllers.Api
{
    [SecureFor("/projects")]
    [DefaultHttpRouteConvention]
    [RoutePrefix("api/projects/{projectid}/activities/{activityid}/todolist")]
    public class TodoListController : RavenDbApiController
    {
        public IEnumerable<TodoList> Get(int projectid, int activityid)
        {
            var project = DbSession
                .Include<Core.Project>(p => p.Activities)
                .Load<Core.Project>(Request.GetCurrentPersonId());

            if (project.Activities.Contains(activityid.ToString()))
            {
                Request.NotFound();
            }
            var activity = DbSession.Load<Core.Activity>(activityid);
            return Mapper.Map<IEnumerable<Core.TodoList>, IEnumerable<TodoList>>(activity.Todos);
        }

        public TodoList Get(int id, int projectid, int activityid)
        {
            var project = DbSession
                           .Include<Core.Project>(p => p.Activities)
                           .Load<Core.Project>(Request.GetCurrentPersonId());

            if (project.Activities.Contains(activityid.ToString()))
            {
                Request.NotFound();
            }
            var activity = DbSession.Load<Core.Activity>(activityid);

            var list = activity.Todos.SingleOrDefault(p => p.Id == id);
            if (list == null)
                Request.NotFound();

            return Mapper.Map<Core.TodoList, TodoList>(list);
        }

        public HttpResponseMessage Post(int projectid, int activityid, TodoList model)
        {
            var project = DbSession
                .Include<Core.Project>(p => p.Activities)
                .Load<Core.Project>(Request.GetCurrentPersonId());

            if (project.Activities.Contains(activityid.ToString()))
            {
                Request.NotFound();
            }
            var activity = DbSession.Load<Core.Activity>(activityid);

            var list = Core.TodoList.Forge(model.Name, model.Description);
            list.Id = activity.GenerateNewTodoListId();
            activity.Todos.Add(list);
            DbSession.SaveChanges();
            return Request.CreateResponse(HttpStatusCode.Created, Mapper.Map<Core.TodoList, TodoList>(list));
        }

        public HttpResponseMessage Put(int projectid, int activityid, TodoList model)
        {
            var project = DbSession
                 .Include<Core.Project>(p => p.Activities)
                 .Load<Core.Project>(Request.GetCurrentPersonId());

            if (project.Activities.Contains(activityid.ToString()))
                Request.NotFound();
            
            var activity = DbSession.Load<Core.Activity>(activityid);

            var list = activity.Todos.SingleOrDefault(p => p.Id == model.Id);
            if (list == null)
                Request.NotFound();
            
            list.Name = model.Name;
            list.Description = model.Description;
            DbSession.SaveChanges();
            return Request.CreateResponse(HttpStatusCode.Created, Mapper.Map<Core.TodoList, TodoList>(list));
        }

        public HttpResponseMessage Delete(int id, int projectid, int activityid)
        {
            var project = DbSession
                  .Include<Core.Project>(p => p.Activities)
                  .Load<Core.Project>(Request.GetCurrentPersonId());

            if (project.Activities.Contains(activityid.ToString()))
                Request.NotFound();
            
            var activity = DbSession.Load<Core.Activity>(activityid);

            var list = activity.Todos.SingleOrDefault(p => p.Id == id);
            if (list == null)
                Request.NotFound();
            
            activity.Todos.Remove(list);
            DbSession.SaveChanges();
            return new HttpResponseMessage(HttpStatusCode.NoContent);
        }
    }
}