using System;
using System.Collections.Generic;
using System.Linq;
using Raven.Bundles.Authorization.Model;
using Raven.Client;
using Raven.Client.Authorization;
using Teamworks.Core.Services;

namespace Teamworks.Core.Extensions
{
    public static class ActivityExtensions
    {
        public static void Delete(this Activity activity,IDocumentSession session)
        {
            session.Query<Discussion>()
                .Where(d => d.Entity == activity.Id).ToList().ForEach(d => d.Delete(session));
            session.Delete(activity);
        }
        
        public static Activity Update(this Activity activity, Activity newEntity, IDocumentSession session)
        {
            var query = session
                .Query<Activity>()
                .Where(a => a.Project == newEntity.Project)
                .ToList();

            if (newEntity.StartDate > DateTimeOffset.MinValue)
                activity.StartDate = newEntity.StartDate;

            activity.Name = newEntity.Name ?? activity.Name;
            activity.Description = newEntity.Description ?? activity.Description;
            if (activity.Duration != newEntity.Duration)
                OffsetDuration(query, activity, newEntity.Duration - activity.Duration);

            if (newEntity.Dependencies != null)
            {
                activity.Dependencies = query.Select(a => a.Id)
                    .Intersect(newEntity.Dependencies)
                    .ToList();

                var lastDependencyToEnd = session.Load<Activity>(activity.Dependencies)
                    .OrderByDescending(a => a.StartDate)
                    .FirstOrDefault();

                if (lastDependencyToEnd != null)
                {
                    activity.StartDateConsecutive =
                        lastDependencyToEnd.StartDateConsecutive.AddSeconds(lastDependencyToEnd.Duration);
                    activity.StartDate =
                        lastDependencyToEnd.StartDate.AddDays(Math.Floor(lastDependencyToEnd.Duration/(8.0*3600)));
                }
            }

            activity.Duration = newEntity.Duration;
            return activity;    
        }

        private static void OffsetDuration(ICollection<Activity> domain, Activity parent, int offset)
        {
            var children = domain.Where(a => a.Dependencies.Contains(parent.Id)).ToList();
            foreach (var child in children)
            {
                child.StartDateConsecutive = child.StartDateConsecutive.AddSeconds(offset);
                domain.Remove(child);
                OffsetDuration(domain, child, offset);
            }
        }
    }
}