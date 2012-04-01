using System;
using Newtonsoft.Json;

namespace Teamworks.Core
{
    public class Reference<T> : IEntity where T: IEntity
        
    {
        #region Implementation of IEntity

        public string Id { get; set; }
        public string Name { get; set; }

        #endregion

        public static implicit operator Reference<T>(T reference)
        {
            return new Reference<T>
                       {
                           Id = reference.Id,
                           Name = reference.Name
                       };
        }
    }

    public abstract class Entity : IEntity
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
        public IUser Creator { get; set; }
    }
}