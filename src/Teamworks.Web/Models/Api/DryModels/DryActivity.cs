using System.ComponentModel.DataAnnotations;

namespace Teamworks.Web.Models.Api.DryModels
{
    public class DryActivity
    {
        public string Id { get; set; }
        [Required]
        [StringLength(512,MinimumLength = 5)]
        public string Name { get; set; }
        public string Description { get; set; }
        public string Project { get; set; }
        public double Duration { get; set; }
    }
}