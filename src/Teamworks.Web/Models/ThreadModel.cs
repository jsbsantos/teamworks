using System.Collections.Generic;
using Teamworks.Core.Projects;
using Teamworks.Web.Models.DryModels;

namespace Teamworks.Web.Models
{
    public class ThreadModel : DryThreadModel
    {
        public IList<MessageModel> Messages { get; set; }
    }
}