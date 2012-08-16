using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Teamworks.Web.ViewModels.Api
{
    public class ActivityViewModel
    {
<<<<<<< HEAD:src/Teamworks.Web/ViewModels/Api/ActivityViewModel.cs
        public int Id { get; set; }
        public int Project { get; set; }
=======
        public string Token { get; set; }
        public int Id { get; set; }
>>>>>>> 871dfa4ed5cef6812243efe6460c3d44b0b432dd:src/Teamworks.Web/Models/Api/Activity.cs

        [Required]
        public string Name { get; set; }
        public string Description { get; set; }

        public int Duration { get; set; }
        public DateTimeOffset StartDate { get; set; }

        public IList<string> Dependencies { get; set; }
    }
}