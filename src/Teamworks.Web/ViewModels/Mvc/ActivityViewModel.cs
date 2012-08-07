using System.Collections.Generic;

namespace Teamworks.Web.ViewModels.Mvc
{
    public class ActivityViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }

        public EntityViewModel ProjectReference { get; set; }
        public IList<Timelog> Timelogs { get; set; }

        #region Nested type: Timelog

        public class Timelog
        {
        }

        #endregion
    }
}