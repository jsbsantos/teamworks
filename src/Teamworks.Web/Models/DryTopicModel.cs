using System;

namespace Teamworks.Web.Models
{
    public class DryTopicModel
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public string Text { get; set; }
        public DateTime Date { get; set; }
        public string Person { get; set; }
    }
}