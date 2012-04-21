using System.Globalization;

namespace Teamworks.Core.People
{
    public class Person : Entity<Person>
    {
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
         
        public Person(string email, string password, string name)
        {
            Email = email;
            Password = EncodePassword(password);
            Name = name;
        }
        private static string EncodePassword(string password)
        {
            return password.GetHashCode().ToString(CultureInfo.InvariantCulture);
        }
        public static bool Authenticate(string id, string password)
        {
            return Session.Load<Person>(id).Password.Equals(EncodePassword(password));
        }
        public string ResetPassword()
        {
            Password = System.Web.Security.Membership.GeneratePassword(8, 0);
            return Password;
        }
    }
}