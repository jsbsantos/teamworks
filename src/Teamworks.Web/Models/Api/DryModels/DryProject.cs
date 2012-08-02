using System.ComponentModel.DataAnnotations;

namespace Teamworks.Web.Models.Api.DryModels
{
    public class DryProject
    {
        public string Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
    }
}