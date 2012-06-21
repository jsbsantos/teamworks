using System.Collections.Generic;
using Teamworks.Web.Models.DryModels;

namespace Teamworks.Web.Models
{
    public class Board : DryBoard
    {
        public IList<Reply> Messages { get; set; }
    }
}