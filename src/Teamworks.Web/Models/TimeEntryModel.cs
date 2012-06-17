using System;
using Teamworks.Web.Models.DryModels;

namespace Teamworks.Web.Models
{
    public class TimeEntryModel
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public long Duration { get; set; }

        public DryPersonModel Person { get; set; }
    }
}