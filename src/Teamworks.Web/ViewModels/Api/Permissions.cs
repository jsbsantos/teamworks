using System.Collections.Generic;

namespace Teamworks.Web.ViewModels.Api
{
    public class Permissions
    {
        public IList<int> Ids { get; set; }
        public IList<string> Emails { get; set; }
    }
}