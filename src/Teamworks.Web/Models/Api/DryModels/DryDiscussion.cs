using System;

namespace Teamworks.Web.Models.Api.DryModels
{
    public class DryDiscussion
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public string Content { get; set; }
        public DateTime Date { get; set; }
        public DryPerson Person { get; set; }
    }
}