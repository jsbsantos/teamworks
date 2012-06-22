using System;

namespace Teamworks.Web.Models.DryModels
{
    public class DryDiscussions
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public string Content { get; set; }
        public DateTime Date { get; set; }
        public DryPerson Person { get; set; }
    }
}