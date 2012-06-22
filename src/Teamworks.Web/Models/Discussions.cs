using System.Collections.Generic;
using Teamworks.Web.Models.DryModels;

namespace Teamworks.Web.Models
{
    public class Discussions : DryDiscussions
    {
        public IList<Message> Messages { get; set; }
    }
}