using System.Collections.Generic;

namespace Teamworks.Core.People
{
    public class TodoList
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public IList<Todo> Todos { get; set; }

        public int LastTodoId { get; private set; }

        public int GenerateNewTodoId()
        {
            return ++LastTodoId;
        }

        public static TodoList Forge(string name, string description)
        {
            return new TodoList
                       {
                           Name = name,
                           Description = description,
                           Todos = new List<Todo>(),
                           LastTodoId=0
                       };
        }
    }
}