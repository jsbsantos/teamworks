using System.Collections.Generic;
using Raven.Bundles.Authorization.Model;

namespace Teamworks.Core
{
    public class Group : Entity
    {
        public IList<IPermission> Permissions { get; set; }

        public static Group Forge(string id)
        {
            return new Group
                       {
                           Id = id,
                           Permissions = new List<IPermission>()
                       };
        }
    }
}