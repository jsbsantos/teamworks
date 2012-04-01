namespace Teamworks.Core
{
    public interface IUser : IEntity
    {
        string Email { get; set; }
        string Username { get; set; }
        string Password { get; set; }
    }
}