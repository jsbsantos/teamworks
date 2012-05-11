namespace Teamworks.Core.Projects {
    public class Task : Entity<Task> {
        public string ProjectId { get; set; }
        public string Description { get; set; }
        public string PreTaskIds { get; set; }
    }
}