using Newtonsoft.Json;

namespace TeamWorks.Core.Entities
{
    public class Entity
    {
        public string Id { get; set; }

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
    }
}