using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Teamworks.Web.Models.Api.DryModels;

namespace Teamworks.Web.Models.Api
{
    public class Activity : DryActivity
    {
        public IList<Timelog> Timelogs { get; set; }
        public IList<TodoList> Todos { get; set; }
        public IList<string> Dependencies { get; set; }
        public IList<string> Discussions { get; set; }
        public IList<string> People { get; set; }

        public string Token { get; set; }
    }
}