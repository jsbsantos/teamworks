using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Teamworks.Web.Attributes.Api.Ordered
{
    public class OrderedFilterProvider : IFilterProvider
    {
        #region IFilterProvider Members

        public IEnumerable<FilterInfo> GetFilters(HttpConfiguration configuration, HttpActionDescriptor actionDescriptor)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException("configuration");
            }

            if (actionDescriptor == null)
            {
                throw new ArgumentNullException("actionDescriptor");
            }

            IEnumerable<OrderedFilterInfo> actionFilters =
                actionDescriptor.GetFilters().Select(i => new OrderedFilterInfo(i, FilterScope.Controller));
            IEnumerable<OrderedFilterInfo> controllerFilters =
                actionDescriptor.ControllerDescriptor.GetFilters().Select(
                    i => new OrderedFilterInfo(i, FilterScope.Controller));

            return controllerFilters.Concat(actionFilters).OrderBy(i => i).Select(i => i.ConvertToAppFilterInfo());
        }

        #endregion
    }
}