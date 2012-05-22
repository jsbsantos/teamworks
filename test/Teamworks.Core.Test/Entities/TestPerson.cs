using Xunit;
using Teamworks.Core.People;

namespace Teamworks.Core.Test.Entities {

    public class TestPerson {
        
        [Fact]
        public void ForgeUser()
        {
            const string email = "mail@mail.com";
            const string username = "username";
            const string password = "password";

            var person = Person.Forge(email, username, password);
            Assert.Equal(email, person.Email);
            Assert.Equal(username, person.Username);
        }

        [Fact]
        public void IsThePasswordCorrect()
        {
            const string email = "mail@mail.com";
            const string username = "username";
            const string password = "password";

            var person = Person.Forge(email, username, password);
            Assert.True(person.IsThePassword(password));
        }
    }
}