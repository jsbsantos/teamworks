using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Teamworks.Web.Models.Api
{
    public class Activity
    {
        public string Id { get; set; }
        [Required]
        [StringLength(512,MinimumLength = 5)]
        public string Name { get; set; }
        public string Description { get; set; }
        public string Project { get; set; }

        public IList<Timelog> Timelogs { get; set; }
        public IList<TodoList> Todos { get; set; }
        public IList<string> Discussions { get; set; }
        public IList<string> People { get; set; }

        public string Token { get; set; }
    }
}