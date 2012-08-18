using System;
using System.Web.Http.Filters;

namespace Teamworks.Web.Attributes.Api.Ordered
{
    public class OrderedFilterInfo : IComparable
    {
        public OrderedFilterInfo(IFilter instance, FilterScope scope)
        {
            Instance = instance;
            Scope = scope;
        }

        public IFilter Instance { get; set; }
        public FilterScope Scope { get; set; }

        #region IComparable Members

        public int CompareTo(object obj)
        {
            if (obj is OrderedFilterInfo)
            {
                return CompareTo(obj as OrderedFilterInfo);
            }
            throw new ArgumentException();
        }

        #endregion

        public FilterInfo ConvertToAppFilterInfo()
        {
            return new FilterInfo(Instance, Scope);
        }

        public int CompareTo(OrderedFilterInfo filter)
        {
            if (Instance is IOrderedFilter)
            {
                var instance = Instance as IOrderedFilter;
                if (filter.Instance is IOrderedFilter)
                {
                    var attr = filter.Instance as IOrderedFilter;
                    return instance.Order.CompareTo(attr.Order);
                }
                return -1;
            }
            // if the passed type is not of OrderedFilterInfo type they are
            // equal else the passed object precedes this.
            return filter.Instance is IOrderedFilter ? 1 : 0;
        }
    }
}