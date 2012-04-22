using Raven.Client;
using Raven.Client.Document;
using Teamworks.Core.Extensions;
using Teamworks.Core.Projects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Teamworks.Core.People;
using System.Collections.Generic;
using Teamworks.Core.Entities;

namespace Teamworks.Tests
{
    
    
    /// <summary>
    ///This is a test class for ProjectTest and is intended
    ///to contain all ProjectTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ProjectTest
    {
        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
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
        [TestInitialize()]
        public void MyTestInitialize()
        {

            IDocumentStore documentStore = new DocumentStore
            {
                ConnectionStringName = "RavenDB"
            }
            .Initialize();

            Local.Data["ravensession"] = documentStore.OpenSession();
        }
        public IDocumentSession Session { get { return Local.Data["ravensession"] as IDocumentSession; } }

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
            Project p = new Project("myproject","description"){Archived = false};
            Project.Add(p);
            Session.SaveChanges();
            Assert.IsFalse(string.IsNullOrEmpty(p.Id));
        }

        /// <summary>
        ///A test for Authenticate
        ///</summary>
        [TestMethod()]
        public void GetTest_successfull_if_properties_are_the_same()
        {
            Project p = new Project("myproject", "description") { Archived = false };
            Project.Add(p);
            Session.SaveChanges();
            Project loaded = Project.FindOne(p.Id);
            Assert.AreEqual(p, loaded);
        }

        /// <summary>
        ///A test for Authenticate
        ///</summary>
        [TestMethod()]
        public void RemoveTest_successfull_if_get_Fails()
        {
            Project p = new Project("myproject", "description") { Archived = false };
            Project.Add(p);
            Session.SaveChanges();

            Project.Remove(p);
            Session.SaveChanges();

            Project loaded = Project.FindOne(p.Id);
            Assert.IsNull(loaded);
        }

        /// <summary>
        ///A test for Load
        ///</summary>
        [TestMethod()]
        public void LoadTest()
        {
            Project p = new Project("myproject", "description") { Archived = false };
            Project.Add(p);
            Session.SaveChanges();
            Project loaded = Project.Load(p.Id);
            Assert.AreEqual(p, loaded);
        }

        /// <summary>
        ///A test for People
        ///</summary>
        [TestMethod()]
        public void PeopleTest()
        {
            Project p = Project.Add(new Project("myproject", "description") { Archived = false });
            var person = Person.Add(new Person("email", "pwd", "name"));
            Session.SaveChanges();
            p.PeopleReference.Add(person);
            Session.SaveChanges();

            var loaded = Project.Load(p.Id);

            Assert.IsTrue(loaded.People.Contains(person));
        }

        /// <summary>
        ///A test for Tasks
        ///</summary>
        [TestMethod()]
        public void TasksTest()
        {
            Project p = Project.Add(new Project("myproject", "description") { Archived = false });
            Session.SaveChanges();
            var task = Task.Add(new Task("task", "desctask", 10, DateTime.Now, p.Id));
            p.TasksReference.Add(task);
            Session.SaveChanges();

            var loaded = Project.Load(p.Id);

            Assert.IsTrue(loaded.Tasks.Contains(task));
            
        }

        /// <summary>
        ///A test for TotalConsumedHours
        ///</summary>
        [TestMethod()]
        public void TotalConsumedHoursTest()
        {
            Project p = Project.Add(new Project("myproject", "description") { Archived = false });
            Session.SaveChanges();
            p.TasksReference.Add(Task.Add(new Task("task", "desctask", 10, DateTime.Now, p.Id){Consumed = 2}));
            p.TasksReference.Add(Task.Add(new Task("task", "desctask", 10, DateTime.Now, p.Id) { Consumed = 3 }));
            Session.SaveChanges();
            var loaded = Project.Load(p.Id);
            var x = loaded.TotalConsumedHours;
            Session.Advanced.Eagerly.ExecuteAllPendingLazyOperations();
            Assert.AreEqual(5,loaded.TotalConsumedHours);

        }

        /// <summary>
        ///A test for TotalEstimatedHours
        ///</summary>
        [TestMethod()]
        public void TotalEstimatedHoursTest()
        {
            Project p = Project.Add(new Project("myproject", "description") { Archived = false });
            Session.SaveChanges();
            p.TasksReference.Add(Task.Add(new Task("task", "desctask", 10, DateTime.Now, p.Id) { Consumed = 2 }));
            p.TasksReference.Add(Task.Add(new Task("task", "desctask", 10, DateTime.Now, p.Id) { Consumed = 3 }));
            Session.SaveChanges();
            var loaded = Project.Load(p.Id);
            var x = loaded.TotalEstimatedHours;
            Session.Advanced.Eagerly.ExecuteAllPendingLazyOperations();
            Assert.AreEqual(20, x);
        }
    }
}
