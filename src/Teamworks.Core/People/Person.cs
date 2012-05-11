using System.Security.Cryptography;
using System.Text;

namespace Teamworks.Core.People {
    public class Person : Entity<Person> {
        public Person(string email, string username, string password) {
            Salt = GenSalt();
            Email = email;
            Username = Name = username;
            Password = EncodePassword(password, Salt);
        }

        private string Salt { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public bool IsThePassword(string password) {
            string other = EncodePassword(password, Salt);
            return System.String.CompareOrdinal(Password, other) == 0;
        }

        private static string EncodePassword(string password, string salt) {
            HashAlgorithm algorithm = new SHA256Managed();
            byte[] plain = Encoding.Unicode.GetBytes(password + salt);
            return Encoding.UTF8.GetString(algorithm.ComputeHash(plain));
        }

        private static string GenSalt() {
            var random = new RNGCryptoServiceProvider();
            var salt = new byte[32]; //256 bits
            random.GetBytes(salt);
            return Encoding.UTF8.GetString(salt);
        }
    }
}