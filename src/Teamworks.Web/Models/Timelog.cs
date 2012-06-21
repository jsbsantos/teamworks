using System;
using Teamworks.Web.Models.DryModels;

namespace Teamworks.Web.Models
{
    public class Timelog
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public long Duration { get; set; }

        public DryPerson Person { get; set; }
    }
}