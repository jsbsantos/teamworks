using System.Collections.Generic;

namespace Teamworks.Core
{
    public class Topic : Entity
    {
        public IList<Entry> Messages { get; set; }

        #region Nested type: Entry

        public class Entry
        {
            public string Message { get; set; }
        }

        #endregion
    }
}