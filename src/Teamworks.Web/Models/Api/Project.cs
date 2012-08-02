using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Teamworks.Web.Models.Api.DryModels;

namespace Teamworks.Web.Models.Api
{
    public class Project : DryProject
    {
        public IList<Person> People { get; set; }
        public IList<Activity> Activities { get; set; }
        public IList<Discussion> Discussions { get; set; }

        public string Token { get; set; }
    }
}