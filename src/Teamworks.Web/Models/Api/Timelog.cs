using System;
using Teamworks.Web.Models.Api.DryModels;

namespace Teamworks.Web.Models.Api
{
    public class Timelog
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Date { get; set; }
        public int Duration { get; set; }

        public DryPerson Person { get; set; }
    }
}