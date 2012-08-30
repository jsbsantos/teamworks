using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Raven.Bundles.Authorization.Model;

namespace Teamworks.Core
{
    public class Person : Entity
    {
        public string Name { get; set; }

        public string Salt { get; private set; }
        public string Password { get; private set; }

        public string Email { get; set; }
        public string Username { get; set; }
        public IList<string> Roles { get; set; }
        public List<OperationPermission> Permissions { get; set; }


        public static Person Forge(string email, string username, string password, string name)
        {
            var salt = GenSalt();
            return new Person
                       {
                           Salt = salt,
                           Email = email,
                           Name = name,
                           Username = username,
                           Roles = new List<string>(),
                           Permissions = new List<OperationPermission>(),
                           Password = EncodePassword(password, salt)
                       };
        }

        public void ChangePassword(string password)
        {
            Password = EncodePassword(password, Salt);
        }

        public bool IsThePassword(string password)
        {
            var other = EncodePassword(password, Salt);
            return string.CompareOrdinal(Password, other) == 0;
        }

        public static string EncodePassword(string password, string salt)
        {
            HashAlgorithm algorithm = new SHA256Managed();
            var plain = Encoding.UTF8.GetBytes(password + salt);
            return Encoding.UTF8.GetString(algorithm.ComputeHash(plain));
        }

        private static string GenSalt()
        {
            var random = new RNGCryptoServiceProvider();
            var salt = new byte[32]; //256 bits
            random.GetBytes(salt);
            return Encoding.UTF8.GetString(salt);
        }
    }
}