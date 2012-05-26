using System.Collections.Generic;
using Teamworks.Core.Projects;

namespace Teamworks.Web.Models
{
    public class TopicModel : DryTopicModel
    {
        public IList<Message> Messages { get; set; }
    }
}