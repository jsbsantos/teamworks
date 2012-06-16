using System.Collections.Generic;

namespace Teamworks.Web.Models
{
    public class ProjectModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public ICollection<TaskModel> Tasks { get; set; }
        public ICollection<ThreadModel> Threads { get; set; }
    }
}