using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Raven.Bundles.Authorization.Model;
using Teamworks.Core.People;

namespace Teamworks.Web.Helpers.Extensions
{
    public static class ListDocumentPermission
    {
        public static List<DocumentPermission> Add(this List<DocumentPermission> list, Person person)
        {
            list.Add(new DocumentPermission()
                         {
                             Allow = true,
                             Operation = "Projects/View",
                             User = person.Id
                         });
            return list;
        }
    }
}