using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Raven.Client;
using Raven.Client.Document;
using Teamworks.Core.Extensions;
using Teamworks.Core.People;
using Teamworks.Core.Projects;

namespace Teamworks.Core.Test {
    /// <summary>
    ///This is a test class for TaskTest and is intended
    ///to contain all TaskTest Unit Tests
    ///</summary>
    [TestClass]
    public class TaskTest {
        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }

        #region Additional test attributes

        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{

        //}

        protected Project project { get; set; }

        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test

        public IDocumentSession Session {
            get { return Local.Data["ravensession"] as IDocumentSession; }
        }

        [TestInitialize]
        public void MyTestInitialize() {
            IDocumentStore documentStore = new DocumentStore
                                           {
                                               ConnectionStringName = "RavenDB"
                                           }
                .Initialize();

            Local.Data["ravensession"] = documentStore.OpenSession();
            project = Project.Add(new Project("myproject", "description") {Archived = false});
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
            var task = new Task{Name = "task", Description = "desctask", DateTime.Now, ProjectId = project.Id};
            Session.Store(task);
            Session.SaveChanges();
            Assert.IsFalse(string.IsNullOrEmpty(task.Id));
        }

        /// <summary>
        ///A test for Authenticate
        ///</summary>
        [TestMethod]
        public void GetTest_successfull_if_properties_are_the_same() {
            var task = new Task("task", "desctask", 10, DateTime.Now, project.Id);
            Task.Add(task);
            Session.SaveChanges();
            Task loaded = Task.Get(task.Id);
            Assert.AreEqual(task, loaded);
        }

        /// <summary>
        ///A test for Authenticate
        ///</summary>
        [TestMethod]
        public void RemoveTest_successfull_if_get_Fails() {
            var task = new Task("task", "desctask", 10, DateTime.Now, project.Id);
            Task.Add(task);
            Session.SaveChanges();
            Task.Remove(task);
            Session.SaveChanges();
            Task loaded = Task.Get(task.Id);
            Assert.IsNull(loaded);
        }

        /// <summary>
        ///A test for Load
        ///</summary>
        [TestMethod]
        public void LoadTest() {
            var task = new Task("task", "desctask", 10, DateTime.Now, project.Id);
            Task.Add(task);
            Session.SaveChanges();
            Task loaded = Task.Load(task.Id);
            Assert.AreEqual(task, loaded);
        }

        /// <summary>
        ///A test for People
        ///</summary>
        [TestMethod]
        public void PeopleTest() {
            var task = new Task("task", "desctask", 10, DateTime.Now, project.Id);
            Task.Add(task);
            Person person = Person.Add(new Person("email", "pwd", "name"));
            Session.SaveChanges();
            task.PeopleReference.Add(person);
            Session.SaveChanges();

            Task loaded = Task.Load(task.Id);

            Assert.IsTrue(loaded.People.Contains(person));
        }

        /// <summary>
        ///A test for Predecessor
        ///</summary>
        [TestMethod]
        public void PredecessorTest() {
            var task = new Task("task", "desctask", 10, DateTime.Now, project.Id);
            Task.Add(task);
            Session.SaveChanges();
            Task pred = Task.Add(new Task("task", "desctask", 10, DateTime.Now, project.Id));
            task.PredecessorReference.Add(task);
            Session.SaveChanges();

            Task loaded = Task.Load(task.Id);

            Assert.IsTrue(loaded.Predecessor.Contains(task));
        }
    }
}