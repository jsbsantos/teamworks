using System.Collections.Generic;
using System.Linq;
using Teamworks.Core.Services;

namespace Teamworks.Core.Business
{
    public class ActivityServices : BusinessService
    {

        public Activity Update(Activity activity)
        {
            var query= DbSession
                .Query<Activity>()
                .Where(a => a.Project == activity.Project)
                .ToList();

            var target = query.FirstOrDefault(a => a.Id == activity.Id);

            if (target == null)
                return null;

            target.Name = activity.Name ?? target.Name;
            target.Description = activity.Description ?? target.Description;
            if (target.Duration != activity.Duration)
            {
                var domain = DbSession.Query<Activity>()
                    .Where(a => a.Project == target.Project).ToList();
                OffsetDuration(domain, target, activity.Duration - target.Duration);
            }

            if (target.Dependencies != null)
            {
                target.Dependencies = activity.Dependencies
               .Intersect(query.Select(a => a.Id))
               .ToList();
            }

            target.Duration = activity.Duration;
            return target;
        }

        //todo test
        public Timelog UpdateTimelog(int activity, Timelog timelog)
        {
            var dummy = DbSession
                .Load<Activity>(activity);
            if (dummy == null)
                return null;

            var target = dummy.Timelogs.Where(t => t.Id == timelog.Id).FirstOrDefault();
            if (target == null)
                return null;

            target.Duration = timelog.Duration;
            target.Description = timelog.Description;
            target.Date = timelog.Date;

            return target;
        }

        public double GetAccumulatedDuration(IEnumerable<Activity> domain, Core.Activity activity, int duration = 0)
        {
            Core.Activity parent = domain.Where(a => activity.Dependencies.Contains(a.Id))
                .OrderByDescending(a => a.Duration).FirstOrDefault();

            if (parent == null)
                return duration;

            return GetAccumulatedDuration(domain, parent, duration + parent.Duration);
        }

        private static void OffsetDuration(ICollection<Activity> domain, Activity parent, int offset)
        {
            var children = domain.Where(a => a.Dependencies.Contains(parent.Id)).ToList();
            foreach (var child in children)
            {
                child.StartDate = child.StartDate.AddMinutes(offset);
                child.Name += offset;
                domain.Remove(child);
                OffsetDuration(domain, child, offset);
            }
        }
    }
}