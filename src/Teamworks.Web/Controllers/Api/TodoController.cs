using System.Linq;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using AttributeRouting;
using AttributeRouting.Web.Http;
using AutoMapper;
using Teamworks.Web.Helpers.Api;
using Teamworks.Web.Models.Api;

namespace Teamworks.Web.Controllers.Api
{
    [DefaultHttpRouteConvention]
    [RoutePrefix("api/todolist/{listid}/todo")]
    public class TodoController : RavenApiController
    {
        public IEnumerable<Todo> Get(int listid)
        {
            var person = DbSession
                .Load<Core.Person>(Request.GetCurrentPersonId());

            var list = person.Todos.SingleOrDefault(p => p.Id == listid);
            if (list != null)
                return Mapper.Map<IEnumerable<Core.Todo>, IEnumerable<Todo>>(list.Todos);

            Request.NotFound();
            return null;
        }

        public Todo Get(int id,int listid)
        {
            var person = DbSession
                .Load<Core.Person>(Request.GetCurrentPersonId());

            var list = person.Todos.Single(p => p.Id == listid);
            return Mapper.Map<Core.Todo, Todo>(list.Todos.Single(t => t.Id == id));
        }

        public HttpResponseMessage Post(int listid, Todo model)
        {
            var person = DbSession
                .Load<Core.Person>(Request.GetCurrentPersonId());
            var list = person.Todos.Single(p => p.Id == listid);

            var todo = Core.Todo.Forge(model.Name, model.Description, model.DueDate);
            todo.Id = list.GenerateNewTodoId();
            list.Todos.Add(todo);
            DbSession.SaveChanges();
            return Request.CreateResponse(HttpStatusCode.Created, Mapper.Map<Core.TodoList, TodoList>(list));
        }
        
        //Não reconhece o PUT sem attr; porquê?
        [PUT("")]
        public HttpResponseMessage Put(int listid, Todo model)
        {
            var person = DbSession
                .Load<Core.Person>(Request.GetCurrentPersonId());
            var list = person.Todos.Single(p => p.Id == listid);
            var todo = list.Todos.Single(t => t.Id == model.Id);

            todo.Name = model.Name;
            todo.Description = model.Description;
            todo.DueDate = model.DueDate;
            todo.Completed = model.Completed;
            DbSession.SaveChanges();

            return Request.CreateResponse(HttpStatusCode.Created, Mapper.Map<Core.TodoList, TodoList>(list));
        }

        public HttpResponseMessage Delete(int id, int listid)
        {
            var person = DbSession
                .Load<Core.Person>(Request.GetCurrentPersonId());
            var list = person.Todos.Single(p => p.Id == listid);
            var todo = list.Todos.Single(t => t.Id == id);

            list.Todos.Remove(todo);
            DbSession.SaveChanges();
            return new HttpResponseMessage(HttpStatusCode.NoContent);
        }
    }
}