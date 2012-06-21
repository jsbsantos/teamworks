using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Raven.Bundles.Authorization.Model;

namespace Teamworks.Core
{
    public class Person : Entity
    {
        public string Salt { get; private set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public IList<string> Roles { get; set; }
        public IList<IPermission> Permissions { get; set; }

        public static Person Forge(string email, string username, string password)
        {
            string salt = GenSalt();
            return new Person
                       {
                           Id = "people/" + username,
                           Salt = salt,
                           Email = email,
                           Name = username,
                           Username = username,
                           Roles = new List<string>(),
                           Permissions = new List<IPermission>(),
                           Password = EncodePassword(password, salt)
                       };
        }

        public bool IsThePassword(string password)
        {
            string other = EncodePassword(password, Salt);
            return string.CompareOrdinal(Password, other) == 0;
        }

        public static string EncodePassword(string password, string salt)
        {
            HashAlgorithm algorithm = new SHA256Managed();
            byte[] plain = Encoding.UTF8.GetBytes(password + salt);
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