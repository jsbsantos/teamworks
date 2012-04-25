namespace Teamworks.Core.Entities
{
    public class Reference<T> : Entity<T> where T : Entity<T>
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