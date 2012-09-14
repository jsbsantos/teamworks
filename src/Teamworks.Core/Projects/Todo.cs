using System;
using System.Collections.Generic;

namespace Teamworks.Core
{
    public class Todo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Completed { get; set; }
        public DateTimeOffset? DueDate { get; set; }
        public string Person { get; set; }

        public static Todo Forge(string name, string description, DateTimeOffset? dueDate)
        {
            return new Todo
            {
                Name = name,
                Description = description ?? "",
                DueDate = dueDate,
                Completed = false
            };
        }
    }
}