using System.ComponentModel.DataAnnotations;

namespace Teamworks.Web.Models.Api
{
    public class Timelog
    {
        public int Id { get; set; }
        public string Description { get; set; }
        [Required]
        public string Date { get; set; }
        public int Duration { get; set; }
    }
}