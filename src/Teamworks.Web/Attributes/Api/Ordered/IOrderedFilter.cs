﻿using System.Web.Http.Filters;

namespace Teamworks.Web.Attributes.Api.Ordered
{
    public interface IOrderedFilter : IFilter
    {
        int Order { get; set; }
    }
}