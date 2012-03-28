using System.Collections.Generic;

namespace TeamWorks.Core.Entities
{
    public class Project : Entity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public IList<string> Tags { get; set; }
    }
}