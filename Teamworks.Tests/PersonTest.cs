using System.Globalization;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Indexes;
using Teamworks.Core.Extensions;
using Teamworks.Core.People;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Teamworks.Tests
{
    
    
    /// <summary>
    ///This is a test class for PersonTest and is intended
    ///to contain all PersonTest Unit Tests
    ///</summary>
    [TestClass()]
    public class PersonTest
    {
        private dynamic testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public dynamic TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }
        #region Additional test attributes
        public IDocumentSession Session { get { return Local.Data["session"] as IDocumentSession; } }

        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {
            IDocumentStore documentStore = new DocumentStore
            {
                ConnectionStringName = "RavenDB"
            }
            .Initialize();

            Local.Data["session"] = documentStore.OpenSession();
        }
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
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
        [TestMethod()]
        public void AddTest_successfull_if_id_is_created()
        {
            Person person = new Person("someemail@email.xp", "password", "username");
            Person.Add(person);
            Session.SaveChanges();
            Assert.IsFalse(string.IsNullOrEmpty(person.Id));
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for Authenticate
        ///</summary>
        [TestMethod()]
        public void GetTest_successfull_if_properties_are_the_same()
        {
            Person person = new Person("someemail@email.xp", "password", "username");
            Person.Add(person);
            Session.SaveChanges();
            Person loaded = Person.FindOne(person.Id);
            Assert.AreEqual(person,loaded);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for Authenticate
        ///</summary>
        [TestMethod()]
        public void AuthenticateTest()
        {
            Person person = new Person("someemail@email.xp","password","username");
            Person.Add(person);
            Session.SaveChanges();
            bool actual = Person.Authenticate(person.Id, "password");
            Assert.AreEqual(true, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for EncodePassword
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Teamworks.Core.dll")]
        public void EncodePasswordTest()
        {
            string password = "password";
            string expected = password.GetHashCode().ToString(CultureInfo.InvariantCulture);
            string actual = Person_Accessor.EncodePassword(password);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }
    }
}
