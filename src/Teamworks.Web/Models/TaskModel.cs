using System.Collections.Generic;

namespace Teamworks.Web.Models
{
    public class TaskModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Project { get; set; }

        public ICollection<TimelogModel> Timelog { get; set; }
    }
}