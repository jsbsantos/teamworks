using System.Security.Cryptography;
using System.Text;

namespace Teamworks.Core.People
{
    public class Person : Entity
    {
        public static Person Forge(string email, string username, string password) {
            var salt = GenSalt();
            return new Person()
                   {
                       Salt = salt,
                       Email = email,
                       Username = username,
                       Password = EncodePassword(password, salt),
                   };
        }

        public string Salt { get; private set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

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