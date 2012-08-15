using System.ComponentModel.DataAnnotations;

namespace Teamworks.Web.ViewModels.Mvc
{
    public class Register
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Name { get; set; }
    }
}