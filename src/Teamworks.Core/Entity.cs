using Raven.Imports.Newtonsoft.Json;
using Teamworks.Core.Services;

namespace Teamworks.Core
{
    public abstract class Entity
    {
        public string Id { get; set; }

        [JsonIgnore]
        public int Identifier
        {
            get { return Id.ToIdentifier(); }
        }

    }
}