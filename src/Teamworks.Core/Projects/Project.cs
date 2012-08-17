using System;
using System.Collections.Generic;
using Raven.Bundles.Authorization.Model;
using Raven.Imports.Newtonsoft.Json;
using Teamworks.Core.Services;

namespace Teamworks.Core
{
    public class Project : Entity
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public bool Archived { get; set; }

        public IList<string> People { get; set; }

        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset CreatedAt { get; set; }

        public static Project Forge(string name, string description, DateTimeOffset? startdate = null)
        {
            return new Project
                       {
                           Name = name,
                           Description = description ?? "",
                           People = new List<string>(),
                           StartDate = startdate ?? DateTimeOffset.UtcNow,
                           CreatedAt = DateTimeOffset.UtcNow
                       };
        }
    }
}