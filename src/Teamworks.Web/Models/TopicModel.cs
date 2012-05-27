using System.Collections.Generic;
using Teamworks.Core.Projects;
using Teamworks.Web.Models.DryModels;

namespace Teamworks.Web.Models
{
    public class TopicModel : DryTopicModel
    {
        public IList<MessageModel> Messages { get; set; }
    }
}