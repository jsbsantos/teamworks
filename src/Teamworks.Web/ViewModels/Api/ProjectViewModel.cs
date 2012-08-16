using System;
using System.ComponentModel.DataAnnotations;

namespace Teamworks.Web.ViewModels.Api
{
    public class ProjectViewModel
    {
        public int Id { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTimeOffset? StartDate { get; set; }
    }
}