using System.Collections;
using System.Linq;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using AttributeRouting;
using AttributeRouting.Web.Http;
using AutoMapper;
using Teamworks.Web.Helpers.Api;
using Teamworks.Web.Models.Api;
using TodoList = Teamworks.Web.Models.Api.TodoList;

namespace Teamworks.Web.Controllers.Api
{
    [DefaultHttpRouteConvention]
    [RoutePrefix("api/todolist")]
    public class TodoListController : RavenApiController
    {
        public IEnumerable<TodoList> Get()
        {
            var person = DbSession
                .Include<Core.Person>(p => p.Todos)
                .Load<Core.Person>(Request.GetCurrentPersonId());

            return Mapper.Map<IEnumerable<Core.TodoList>, IEnumerable<TodoList>>(person.Todos);
        }

        public TodoList Get(int id)
        {
            var person = DbSession
                .Load<Core.Person>(Request.GetCurrentPersonId());

            var list = person.Todos.SingleOrDefault(p => p.Id == id);
            if (list != null)
                return Mapper.Map<Core.TodoList, TodoList>(list);

            Request.NotFound();
            return null;
        }

        public HttpResponseMessage Post(TodoList model)
        {
            var person = DbSession
                .Load<Core.Person>(Request.GetCurrentPersonId());

            var list = Core.TodoList.Forge(model.Name, model.Description);
            list.Id = person.GenerateNewTodoListId();
            person.Todos.Add(list);
            DbSession.SaveChanges();
            return Request.CreateResponse(HttpStatusCode.Created, Mapper.Map<Core.TodoList, TodoList>(list));
        }

        public HttpResponseMessage Put(TodoList model)
        {
            var person = DbSession
                .Load<Core.Person>(Request.GetCurrentPersonId());

            var list = person.Todos.SingleOrDefault(p => p.Id == model.Id);
            if (list != null)
            {
                list.Name = model.Name;
                list.Description = model.Description;
                DbSession.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.Created, Mapper.Map<Core.TodoList, TodoList>(list));
            }

            Request.NotFound();
            return null;
        }

        public HttpResponseMessage Delete(int id)
        {
            var person = DbSession
                .Load<Core.Person>(Request.GetCurrentPersonId());

            var list = person.Todos.SingleOrDefault(p => p.Id == id);
            if (list != null)
            {
                person.Todos.Remove(list);
                DbSession.SaveChanges();
                return new HttpResponseMessage(HttpStatusCode.NoContent);
            }
            Request.NotFound();
            return null;
        }
    }
}