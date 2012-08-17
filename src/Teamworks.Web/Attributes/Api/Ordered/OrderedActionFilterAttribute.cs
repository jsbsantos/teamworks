using System.Web.Http.Filters;

namespace Teamworks.Web.Attributes.Api.Ordered
{
    public class OrderedActionFilterAttribute : ActionFilterAttribute, IOrderedFilter
    {
        public OrderedActionFilterAttribute()
        {
            Order = 0;
        }

        public OrderedActionFilterAttribute(int priority)
        {
            Order = priority;
        }

        #region IOrderedFilter Members

        public int Order { get; set; }

        #endregion
    }
}