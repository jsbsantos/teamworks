using System;
using System.Collections.Generic;

namespace Teamworks.Web.ViewModels.Mvc
{
    public class RegisterTimelogsViewModel
    {
        public RegisterTimelogsViewModel()
        {
            Options = new List<Typeahead>();
            Timelogs = new List<Timelog>();
        }

        public IList<Timelog> Timelogs { get; set; }
        public IList<Typeahead> Options { get; set; }

        #region Nested type: Timelog

        public class Timelog
        {
            public EntityViewModel Project { get; set; }
            public EntityViewModel Activity { get; set; }

            public int Duration { get; set; }
            public string Description { get; set; }
            public DateTime Date { get; set; }
        }

        #endregion

        #region Nested type: Typeahead

        public class Typeahead
        {
            public EntityViewModel Activity { get; set; }
            public EntityViewModel Project { get; set; }
        }

        #endregion
    }
}