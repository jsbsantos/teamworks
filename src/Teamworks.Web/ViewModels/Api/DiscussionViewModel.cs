using System;
using System.Collections.Generic;

namespace Teamworks.Web.ViewModels.Api
{
    public class DiscussionViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public DateTimeOffset Date { get; set; }
        public IList<MessageViewModel> Messages { get; set; }
    }
}