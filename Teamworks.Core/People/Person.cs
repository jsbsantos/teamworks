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
            Username = Name = name;
        }

        public static string EncodePassword(string password)
        {
            return password.GetHashCode().ToString(CultureInfo.InvariantCulture);
        }
        public static bool Authenticate(string id, string password)
        {
            return Session.Load<Person>(id).Password.Equals(EncodePassword(password));
        }
        public string ResetPassword()
        {
            var pwd = System.Web.Security.Membership.GeneratePassword(8, 0);
            Password = EncodePassword(pwd);
            return pwd;
        }
    }
}