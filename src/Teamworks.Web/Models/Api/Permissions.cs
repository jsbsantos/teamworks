using System.Collections.Generic;

namespace Teamworks.Web.Models.Api
{
    public class Permissions
    {
        public IList<int> Ids { get; set; }
        public IList<string> Emails { get; set; }
    }
}