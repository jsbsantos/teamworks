using System;
using System.Collections.Generic;
using System.Linq;
using Teamworks.Core.Services;

namespace Teamworks.Core.Business
{
    public class ActivityServices : BusinessService
    {
        public bool AddPrecedence(int activity, int project, int[] precedences)
        {
            var query = DbSession
            .Query<Activity>()
            .Where(a => a.Project == project.ToId("project"))
            .ToList();

            var target = query.FirstOrDefault(a => a.Identifier == activity);

            if (query.Count == 0 || target == null)
                return false;

            target.Dependencies = target.Dependencies.Union(precedences
                .Select(p => p.ToId("activity"))
                .Intersect(query.Select(a => a.Id)))
                .ToList();
            return true;
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

        public bool Update(Activity activity)//o que receber aqui? todos os campos editavies? viewmodel?
        {
            var activity = DbSession
                .Load<Core.Activity>(model.Id);

            if(activity==null)
                return false;

            activity.Name = model.Name ?? activity.Name;
            activity.Description = model.Description ?? activity.Description;
            if (activity.Duration != model.Duration)
            {
                List<Core.Activity> domain = DbSession.Query<Core.Activity>()
                    .Where(a => a.Project == activity.Project).ToList();
                OffsetDuration(domain, activity, model.Duration - activity.Duration);
            }

            activity.Duration = model.Duration;
            return true;
        }

        private double GetAccumulatedDuration(List<Core.Activity> domain, Core.Activity activity, int duration = 0)
        {
            Core.Activity parent = domain.Where(a => activity.Dependencies.Contains(a.Id))
                .OrderByDescending(a => a.Duration).FirstOrDefault();

            if (parent == null)
                return duration;

            return /*parent.Dependencies.Count == 0
                       ? duration
                       : */
                GetAccumulatedDuration(domain, parent, duration + parent.Duration);
        }

        private void OffsetDuration(List<Core.Activity> domain, Core.Activity parent, int offset)
        {
            List<Core.Activity> children = domain.Where(a => a.Dependencies.Contains(parent.Id)).ToList();
            foreach (Core.Activity child in children)
            {
                child.StartDate = child.StartDate.AddMinutes(offset);
                child.Name += offset;
                domain.Remove(child);
                OffsetDuration(domain, child, offset);
            }
        }

        public void AddTimelog(int activity, Timelog timelog)
        {
            
        }

        public void RemoveTimelog(int activity, int timelog)
        {

        }

        public void AddPerson(int activity, Person person)
        {

        }

        public void RemovePerson(int activity, int person)
        {

        }

        public void AddDiscussion(int activity, Discussion discussion)
        {

        }
        public void RemoveDiscussion(int activity, Discussion discussion)
        {

        }

    }
}