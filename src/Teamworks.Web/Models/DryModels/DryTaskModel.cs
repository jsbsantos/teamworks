using System.ComponentModel.DataAnnotations;

namespace Teamworks.Web.Models.DryModels
{
    public class DryTaskModel
    {
        public string Id { get; set; }
        [Required]
        [StringLength(512,MinimumLength = 5)]
        public string Name { get; set; }
        public string Description { get; set; }
        public string Project { get; set; }
    }
}