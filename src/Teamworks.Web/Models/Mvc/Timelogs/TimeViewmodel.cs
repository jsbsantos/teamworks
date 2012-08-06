using System.Collections.Generic;
using Teamworks.Web.Models.Api;
using Teamworks.Web.ViewModels;
using Teamworks.Web.ViewModels.Mvc;

namespace Teamworks.Web.Models.Mvc.Timelogs
{
    public class TimeViewModel
    {
        public IEnumerable<TypeaheadViewModel> Source { set; get; }
        public List<Timelog> Timelogs { get; set; }
    }
}