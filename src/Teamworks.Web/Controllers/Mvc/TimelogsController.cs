using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Teamworks.Core;
using Teamworks.Web.Helpers.AutoMapper;
using Teamworks.Web.ViewModels.Mvc;

namespace Teamworks.Web.Controllers.Mvc
{
    public class TimelogsController : RavenController
    {
        [HttpGet]
        [ActionName("View")]
        public ActionResult Index()
        {
            List<Activity> activities = DbSession.Query<Activity>()
                .Customize(c => c.Include<Activity>(a => a.Project))
                .ToList();

            var vm = new RegisterTimelogsViewModel();
            foreach (Activity act in activities)
            {
                Activity activity = act;
                var project = DbSession.Load<Project>(activity.Project);
                var option = new RegisterTimelogsViewModel.Typeahead
                                 {
                                     Activity = activity.MapTo<EntityViewModel>(),
                                     Project = project.MapTo<EntityViewModel>()
                                 };

                vm.Options.Add(option);
                vm.Timelogs = vm.Timelogs.Concat(activity.Timelogs.MapTo<RegisterTimelogsViewModel.Timelog>()
                                                     .Select(a =>
                                                                 {
                                                                     a.Activity = activity.MapTo<EntityViewModel>();
                                                                     a.Project = project.MapTo<EntityViewModel>();
                                                                     return a;
                                                                 })).ToList();
            }
            return View(vm);
        }
    }
}