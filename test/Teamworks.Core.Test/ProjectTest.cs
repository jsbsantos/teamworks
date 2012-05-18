
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Raven.Client;
using Raven.Client.Document;
using Teamworks.Core.Extensions;
using Teamworks.Core.People;
using Teamworks.Core.Projects;
using Teamworks.Core.Services;

namespace Teamworks.Core.Test {
    /// <summary>
    ///This is a test class for ProjectTest and is intended
    ///to contain all ProjectTest Unit Tests
    ///</summary>
    [TestClass]
    public class ProjectTest {
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
        }

        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //

        #endregion
        /*
        /// <summary>
        ///A test for Authenticate
        ///</summary>
        [TestMethod]
        public void AddTest_successfull_if_id_is_created() {
            var p = new Project("myproject", "description") {Archived = false};
            Project.Add(p);
            Session.SaveChanges();
            Assert.IsFalse(string.IsNullOrEmpty(p.Id));
        }

        /// <summary>
        ///A test for Authenticate
        ///</summary>
        [TestMethod]
        public void GetTest_successfull_if_properties_are_the_same() {
            var p = new Project("myproject", "description") {Archived = false};
            Project.Add(p);
            Session.SaveChanges();
            Project loaded = Project.Get(p.Id);
            Assert.AreEqual(p, loaded);
        }

        /// <summary>
        ///A test for Authenticate
        ///</summary>
        [TestMethod]
        public void RemoveTest_successfull_if_get_Fails() {
            var p = new Project("myproject", "description") {Archived = false};
            Project.Add(p);
            Session.SaveChanges();

            Project.Remove(p);
            Session.SaveChanges();

            Project loaded = Project.Get(p.Id);
            Assert.IsNull(loaded);
        }

        /// <summary>
        ///A test for Load
        ///</summary>
        [TestMethod]
        public void LoadTest() {
            var p = new Project("myproject", "description") {Archived = false};
            Project.Add(p);
            Session.SaveChanges();
            Project loaded = Project.Load(p.Id);
            Assert.AreEqual(p, loaded);
        }

        /// <summary>
        ///A test for People
        ///</summary>
        [TestMethod]
        public void PeopleTest() {
            Project p = Project.Add(new Project("myproject", "description") {Archived = false});
            Person person = Person.Add(new Person("email", "pwd", "name"));
            Session.SaveChanges();
            p.PeopleReference.Add(person);
            Session.SaveChanges();

            Project loaded = Project.Load(p.Id);

            Assert.IsTrue(loaded.People.Contains(person));
        }

        /// <summary>
        ///A test for Tasks
        ///</summary>
        [TestMethod]
        public void TasksTest() {
            Project p = Project.Add(new Project("myproject", "description") {Archived = false});
            Session.SaveChanges();
            Task task = Task.Add(new Task("task", "desctask", 10, DateTime.Now, p.Id));
            p.TasksReference.Add(task);
            Session.SaveChanges();

            Project loaded = Project.Load(p.Id);

            Assert.IsTrue(loaded.Tasks.Contains(task));
        }

        /// <summary>
        ///A test for TotalConsumedHours
        ///</summary>
        [TestMethod]
        public void TotalConsumedHoursTest() {
            Project p = Project.Add(new Project("myproject", "description") {Archived = false});
            Session.SaveChanges();
            p.TasksReference.Add(Task.Add(new Task("task", "desctask", 10, DateTime.Now, p.Id) {Consumed = 2}));
            p.TasksReference.Add(Task.Add(new Task("task", "desctask", 10, DateTime.Now, p.Id) {Consumed = 3}));
            Session.SaveChanges();
            Project loaded = Project.Load(p.Id);
            long x = loaded.TotalConsumedHours;
            Session.Advanced.Eagerly.ExecuteAllPendingLazyOperations();
            Assert.AreEqual(5, loaded.TotalConsumedHours);
        }

        /// <summary>
        ///A test for TotalEstimatedHours
        ///</summary>
        [TestMethod]
        public void TotalEstimatedHoursTest() {
            Project p = Project.Add(new Project("myproject", "description") {Archived = false});
            Session.SaveChanges();
            p.TasksReference.Add(Task.Add(new Task("task", "desctask", 10, DateTime.Now, p.Id) {Consumed = 2}));
            p.TasksReference.Add(Task.Add(new Task("task", "desctask", 10, DateTime.Now, p.Id) {Consumed = 3}));
            Session.SaveChanges();
            Project loaded = Project.Load(p.Id);
            long x = loaded.TotalEstimatedHours;
            Session.Advanced.Eagerly.ExecuteAllPendingLazyOperations();
            Assert.AreEqual(20, x);
        }
        */
    }
}