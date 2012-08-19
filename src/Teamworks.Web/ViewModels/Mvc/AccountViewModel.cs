using System.ComponentModel.DataAnnotations;

namespace Teamworks.Web.ViewModels.Mvc
{
    public class AccountViewModel
    {
        [Required(AllowEmptyStrings = false)]
        [Display(Name = "Username")]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool Persist { get; set; }

        public class Register
        {
            [Required]
            public string Email { get; set; }

            [Required]
            [StringLength(256, MinimumLength = 6)]
            public string Username { get; set; }

            [Required(AllowEmptyStrings = false)]
            public string Name { get; set; }

            [Required]
            [StringLength(256, MinimumLength = 8, ErrorMessage = "The password must have more than 8 characters.")]
            public string Password { get; set; }

            public string ReturnUrl { get; set; }
        }
    }
}