using System;

namespace Teamworks.Web.ViewModels.Mvc
{
    public class TimelogViewModel
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public DateTimeOffset Date { get; set; }
        public int Duration { get; set; }

        public EntityViewModel Person { get; set; }
    }
}