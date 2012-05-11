using System;
using Newtonsoft.Json;
using Raven.Client;
using Raven.Client.Linq;
using Teamworks.Core.Extensions;
using Teamworks.Core.People;

namespace Teamworks.Core {
    public abstract class Entity<T> {
        
        public string Id { get; set; }
        public string Name { get; set; }

        [JsonIgnore]
        public int Identifier {
            get {
                int i;
                if (string.IsNullOrEmpty(Id) || (i = Id.IndexOf('/')) < 0) {
                    return 0;
                }

                int id;
                return int.TryParse(Id.Substring(i + 1, Id.Length - i - 1), out id) ? id : 0;
            }
        }
    }
}