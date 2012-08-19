using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Teamworks.Web.ViewModels.Api
{
    public class TimelogViewModel
    {
        public int Id { get; set; }

        [JsonProperty(Required = Required.Always)]
        public string Description { get; set; }

        [JsonProperty(Required = Required.Always)]
        public string Date { get; set; }

        [JsonProperty(Required=Required.Always)]
        public int Duration { get; set; }

        public int Activity { get; set; }
    }
}