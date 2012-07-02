using System;

namespace Teamworks.Web.Models.DryModels
{
    public class DryThreadModel
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public string Entity { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }
        public DryPersonModel Person { get; set; }
    }
}