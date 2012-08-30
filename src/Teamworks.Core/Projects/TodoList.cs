using System;
using System.Collections.Generic;

namespace Teamworks.Core
{
    public class TodoList
    {
        public int Id { get; set; }
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
                           LastTodoId = 0
                       };
        }
    }

    public class Todo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Completed { get; set; }
        public DateTimeOffset? DueDate { get; set; }

        public static Todo Forge(string name, string description, DateTimeOffset? dueDate)
        {
            return new Todo
            {
                Name = name,
                Description = description,
                DueDate = dueDate,
                Completed = false
            };
        }
    }
}