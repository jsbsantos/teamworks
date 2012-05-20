using System;

namespace Teamworks.Core.Projects
{
    public class Timelog
    {
        public string Id { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public float Duration { get; set; }

        public string Person { get; set; }
    }
}