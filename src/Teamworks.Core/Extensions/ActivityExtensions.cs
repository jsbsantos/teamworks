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
        public static Activity Update(this Activity activity, Activity newEntity, IDocumentSession DbSession)
        {
            var query = DbSession
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
            }

            activity.Duration = newEntity.Duration;
            return activity;
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