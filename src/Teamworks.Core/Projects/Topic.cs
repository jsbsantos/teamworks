using System.Collections.Generic;

namespace Teamworks.Core {
    public class Topic : Entity {
        public class Entry
        {
            public string Message { get; set; }
        }

        public IList<Entry> Messages { get; set; }
    }
}