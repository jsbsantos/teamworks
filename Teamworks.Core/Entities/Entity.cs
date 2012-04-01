using System;
using Newtonsoft.Json;

namespace Teamworks.Core.Entities
{
    public abstract class Entity
    {
        #region Implementation of IEntity

        public string Id { get; set; }
        public string Name { get; set; }

        #endregion

        [JsonIgnore]
        public int Identifier
        {
            get
            {
                int i;
                if (string.IsNullOrEmpty(Id) || (i = Id.IndexOf('/')) < 0)
                    return 0;

                int id;
                return int.TryParse(Id.Substring(i + 1, Id.Length - i - 1), out id) ? id : 0;
            }
        }
        
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public Reference<User> Creator { get; set; }
    }

    public class User : Entity
    {
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}