using AttributeRouting;
using AttributeRouting.Web.Http;
using Teamworks.Web.Attributes.Api;

namespace Teamworks.Web.Controllers.Api
{
    [Secure("/projects")]
    [DefaultHttpRouteConvention]
    [RoutePrefix("api/projects/{projectid}/activities/{activityid}/todolist/{listid}/todo")]
    public class TodoController : AppApiController
    {
        /*
        public IEnumerable<TodoViewModel> Get(int id, int projectid, int activityid)
        {
            var project = DbSession
                            .Include<Core.ProjectViewModel>(p => p.Activities)
                            .Load<Core.ProjectViewModel>(Request.GetCurrentPersonId());

            if (project.Activities.Contains(activityid.ToString()))
            {
                Request.NotFound();
            }
            var activity = DbSession.Load<Core.ActivityViewModel>(activityid);

            var list = activity.Todos.SingleOrDefault(p => p.Id == id);
            if (list == null)
                Request.NotFound();

            return Mapper.Map<IEnumerable<Core.TodoViewModel>, IEnumerable<TodoViewModel>>(list.Todos);
        }

        public TodoViewModel Get(int id, int projectid, int activityid, int listid)
        {
            var project = DbSession
                            .Include<Core.ProjectViewModel>(p => p.Activities)
                            .Load<Core.ProjectViewModel>(Request.GetCurrentPersonId());

            if (project.Activities.Contains(activityid.ToString()))
            {
                Request.NotFound();
            }
            var activity = DbSession.Load<Core.ActivityViewModel>(activityid);

            var list = activity.Todos.Single(p => p.Id == listid);
            return Mapper.Map<Core.TodoViewModel, TodoViewModel>(list.Todos.Single(t => t.Id == id));
        }

        public HttpResponseMessage Post(int projectid, int activityid, int listid, TodoViewModel model)
        {
            var project = DbSession
                            .Include<Core.ProjectViewModel>(p => p.Activities)
                            .Load<Core.ProjectViewModel>(Request.GetCurrentPersonId());

            if (project.Activities.Contains(activityid.ToString()))
            {
                Request.NotFound();
            }
            var activity = DbSession.Load<Core.ActivityViewModel>(activityid);

            var list = activity.Todos.Single(p => p.Id == listid);

            var todo = Core.TodoViewModel.Forge(model.Name, model.Description, model.DueDate);
            todo.Id = list.GenerateNewTodoId();
            list.Todos.Add(todo);
            DbSession.SaveChanges();
            return Request.CreateResponse(HttpStatusCode.Created, Mapper.Map<Core.TodoList, TodoList>(list));
        }
        
        //Não reconhece o PUT sem attr; porquê?
        [PUT("")]
        public HttpResponseMessage Put(int projectid, int activityid, int listid, TodoViewModel model)
        {
            var project = DbSession
                            .Include<Core.ProjectViewModel>(p => p.Activities)
                            .Load<Core.ProjectViewModel>(Request.GetCurrentPersonId());

            if (project.Activities.Contains(activityid.ToString()))
            {
                Request.NotFound();
            }
            var activity = DbSession.Load<Core.ActivityViewModel>(activityid);
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
                                        .Include<Core.ProjectViewModel>(p => p.Activities)
                                        .Load<Core.ProjectViewModel>(Request.GetCurrentPersonId());

            if (project.Activities.Contains(activityid.ToString()))
            {
                Request.NotFound();
            }
            var activity = DbSession.Load<Core.ActivityViewModel>(activityid);
            var list = activity.Todos.Single(p => p.Id == listid);
            var todo = list.Todos.Single(t => t.Id == id);

            list.Todos.Remove(todo);
            DbSession.SaveChanges();
            return new HttpResponseMessage(HttpStatusCode.NoContent);
        }
        */
    }
}