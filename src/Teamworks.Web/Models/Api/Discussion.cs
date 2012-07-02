using System.Collections.Generic;
using Teamworks.Web.Models.Api.DryModels;

namespace Teamworks.Web.Models.Api
{
    public class Discussion : DryDiscussion
    {
        public IList<Message> Messages { get; set; }
    }
}