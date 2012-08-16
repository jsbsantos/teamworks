using System.ComponentModel.DataAnnotations;

namespace Teamworks.Web.ViewModels.Mvc
{
    public class ProfileViewModel
    {
        public bool IsMyProfile { get; set; }
        public PersonViewModel PersonDetails { get; set; }

        public class Input
        {
            [Required(AllowEmptyStrings = false)]
            public string Name { get; set; }
            [Required]
            [RegularExpression(".+\\@.+\\..+", ErrorMessage = "Please enter a valid email address.")]
            public string Email { get; set; }

        }
    }
}