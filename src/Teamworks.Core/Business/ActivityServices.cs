using System.Collections.Generic;
using System.Linq;
using Teamworks.Core.Services;

namespace Teamworks.Core.Business
{
    public class ActivityServices : BusinessService
    {
/*        public Activity AddPrecedence(int activity, int project, int[] precedences)
        {
            var query = DbSession
                .Query<Activity>()
                .Customize(c => c.Include<Activity>(a => a.Project))
                .Where(a => a.Project == project.ToId("project"))
                .ToList();

            DbSession.Load<Project>(project); 

            var target = query.FirstOrDefault(a => a.Identifier == activity);

            if (query.Count == 0 || target == null)
                return null;

            target.Dependencies = target.Dependencies.Union(precedences
                                                                .Select(p => p.ToId("activity"))
                                                                .Intersect(query.Select(a => a.Id)))
                .ToList();
            return target;
        }

        public bool RemovePrecedence(int activity, int project, int[] precedences)
        {
            var query = DbSession
                .Query<Activity>()
                .Where(a => a.Project == project.ToId("project"))
                .ToList();

            var target = query.FirstOrDefault(a => a.Identifier == activity);

            if (query.Count == 0 || target == null)
                return false;

            target.Dependencies = target.Dependencies.Except(precedences
                                                                 .Select(p => p.ToId("activity"))
                                                                 .Intersect(query.Select(a => a.Id)))
                .ToList();
            return true;
        }

        public bool SetPrecedence(int activity, int project, int[] precedences)
        {
            var query = DbSession
                .Query<Activity>()
                .Where(a => a.Project == project.ToId("project"))
                .ToList();

            var target = query.FirstOrDefault(a => a.Identifier == activity);

            if (query.Count == 0 || target == null)
                return false;

            target.Dependencies = precedences
                .Select(p => p.ToId("activity"))
                .Intersect(query.Select(a => a.Id))
                .ToList();
            return true;
        }
        */
        //todo adicionar alteração de precedencias
        public Activity Update(Activity activity) //o que receber aqui? todos os campos editavies? viewmodel?
        {
            var target = DbSession
                .Load<Activity>(activity.Id);

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