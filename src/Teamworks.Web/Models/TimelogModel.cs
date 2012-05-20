using System;

namespace Teamworks.Web.Models
{
    public class TimelogModel
    {
        public string Id { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public float Duration { get; set; }

        public string Person { get; set; }
    }
}