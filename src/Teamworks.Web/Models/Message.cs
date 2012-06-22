using System;
using Teamworks.Web.Models.DryModels;

namespace Teamworks.Web.Models
{
    public class Message
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }
        public DryPerson Person { get; set; }
    }
}