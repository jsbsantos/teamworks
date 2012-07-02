using System;
using Teamworks.Web.Models.DryModels;

namespace Teamworks.Web.Models.Api
{
    public class Timelog
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Date { get; set; }
        public long Duration { get; set; }

        public DryPerson Person { get; set; }
    }
}