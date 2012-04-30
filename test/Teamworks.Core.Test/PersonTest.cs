using System.Globalization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Raven.Client;
using Raven.Client.Document;
using Teamworks.Core.Extensions;
using Teamworks.Core.People;

namespace Teamworks.Core.Test {
    /// <summary>
    ///This is a test class for PersonTest and is intended
    ///to contain all PersonTest Unit Tests
    ///</summary>
    [TestClass]
    public class PersonTest {
        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }

        #region Additional test attributes

        public IDocumentSession Session {
            get { return Local.Data["ravensession"] as IDocumentSession; }
        }

        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{

        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        [TestInitialize]
        public void MyTestInitialize() {
            IDocumentStore documentStore = new DocumentStore
                                           {
                                               ConnectionStringName = "RavenDB"
                                           }
                .Initialize();

            Local.Data["ravensession"] = documentStore.OpenSession();
        }

        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //

        #endregion

        /// <summary>
        ///A test for Authenticate
        ///</summary>
        [TestMethod]
        public void AddTest_successfull_if_id_is_created() {
            var person = new Person("someemail@email.xp", "password", "username");
            Person.Add(person);
            Session.SaveChanges();
            Assert.IsFalse(string.IsNullOrEmpty(person.Id));
        }

        /// <summary>
        ///A test for Authenticate
        ///</summary>
        [TestMethod]
        public void GetTest_successfull_if_properties_are_the_same() {
            var person = new Person("someemail@email.xp", "password", "username");
            Person.Add(person);
            Session.SaveChanges();
            Person loaded = Person.Get(person.Id);
            Assert.AreEqual(person, loaded);
        }

        /// <summary>
        ///A test for Authenticate
        ///</summary>
        [TestMethod]
        public void RemoveTest_successfull_if_get_returns_null() {
            var person = new Person("someemail@email.xp", "password", "username");
            Person.Add(person);
            Session.SaveChanges();
            Person.Remove(person);
            Session.SaveChanges();

            Person loaded = Person.Get(person.Id);
            Assert.IsNull(loaded);
        }

        /// <summary>
        ///A test for Authenticate
        ///</summary>
        [TestMethod]
        public void AuthenticateTest() {
            var person = new Person("someemail@email.xp", "password", "username");
            Person.Add(person);
            Session.SaveChanges();
            bool actual = Person.Authenticate(person.Id, "password");
            Assert.AreEqual(true, actual);
        }

        /// <summary>
        ///A test for EncodePassword
        ///</summary>
        [TestMethod]
        [DeploymentItem("Teamworks.Core.dll")]
        public void EncodePasswordTest() {
            string password = "password";
            string expected = password.GetHashCode().ToString(CultureInfo.InvariantCulture);
            string actual = Person.EncodePassword(password);
            Assert.AreEqual(expected, actual);
        }
    }
}