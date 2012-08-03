using Xunit;

namespace Teamworks.Core.Unittest.Entities {

    public class PersonUnittest {
        
        [Fact]
        public void ForgePerson()
        {
            const string email = "mail@mail.com";
            const string username = "username";
            const string password = "password";
            const string name = "name";

            var person = Core.Person.Forge(email, username, password,name);
            Assert.Equal(email, person.Email);
            Assert.Equal(username, person.Username);
        }

        [Fact]
        public void IsThePasswordCorrect()
        {
            const string email = "mail@mail.com";
            const string username = "username";
            const string password = "password";
            const string name = "name";

            var person = Core.Person.Forge(email, username, password, name);
            Assert.True(person.IsThePassword(password));
        }
    }
}
