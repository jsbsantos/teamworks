using System.Collections.Generic;

namespace Teamworks.Web.Models
{
    public class Project
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public ICollection<Task> Tasks { get; set; }
        public ICollection<Board> Threads { get; set; }
    }
}