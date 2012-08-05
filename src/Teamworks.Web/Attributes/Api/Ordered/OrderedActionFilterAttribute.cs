using System.Web.Http.Filters;

namespace Teamworks.Web.Attributes.Api.Ordered
{
    public class OrderedActionFilterAttribute : ActionFilterAttribute, IOrderedFilter
    {
        public OrderedActionFilterAttribute()
        {
            Priority = 0;
        }

        public OrderedActionFilterAttribute(int priority)
        {
            Priority = priority;
        }

        #region IOrderedFilter Members

        public int Priority { get; set; }

        #endregion
    }
}