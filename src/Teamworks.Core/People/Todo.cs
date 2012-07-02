using System;
using System.Security.Cryptography;
using System.Text;
using Raven.Bundles.Authorization.Model;

namespace Teamworks.Core.People
{
    public class Todo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Completed { get; set; }
        public DateTime DueDate { get; set; }

        public static Todo Forge(string name, string description, DateTime dueDate)
        {
            return new Todo()
                       {
                           Name = name,
                           Description = description,
                           DueDate = dueDate,
                           Completed = false
                       };
        }
    }
}