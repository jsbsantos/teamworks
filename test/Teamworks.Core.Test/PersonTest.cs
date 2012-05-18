using Microsoft.VisualStudio.TestTools.UnitTesting;
using Teamworks.Core.People;

namespace Teamworks.Core.Test {

    [TestClass]
    public class PersonTest {
        
        [TestMethod]
        public void Forge_User_Test()
        {
            const string email = "mail@mail.com";
            const string username = "username";
            const string password = "password";

            var person = Person.Forge(email, username, password);
            Assert.AreEqual(email, person.Email);
            Assert.AreEqual(username, person.Username);
        }

        [TestMethod]
        public void Is_The_Password_Test()
        {
            const string email = "mail@mail.com";
            const string username = "username";
            const string password = "password";

            var person = Person.Forge(email, username, password);
            Assert.IsTrue(person.IsThePassword(password));
        }
    }
}