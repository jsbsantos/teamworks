using System;

namespace Teamworks.Web.Models.DryModels
{
    public class DryTopicModel
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public string Project { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }
        public DryPersonModel Person { get; set; }
    }
}