namespace Teamworks.Core.Entities
{
    public class Reference<T> : Entity where T : Entity
    {
        public static implicit operator Reference<T>(T reference)
        {
            return new Reference<T>
                       {
                           Id = reference.Id,
                           Name = reference.Name
                       };
        }
    }
}