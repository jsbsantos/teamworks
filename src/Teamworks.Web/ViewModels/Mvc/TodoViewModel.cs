using System;

namespace Teamworks.Web.ViewModels.Mvc
{
    public class TodoViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Completed { get; set; }
        public DateTimeOffset? DueDate { get; set; }
    
        public class Output : TodoViewModel
        {
            public EntityViewModel Person { get; set; }
        }
    }
}