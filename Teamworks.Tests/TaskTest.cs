using Raven.Client;
using Raven.Client.Document;
using Teamworks.Core.Extensions;
using Teamworks.Core.Projects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using Teamworks.Core.People;
using Teamworks.Core.Entities;

namespace Teamworks.Tests
{
    
    
    /// <summary>
    ///This is a test class for TaskTest and is intended
    ///to contain all TaskTest Unit Tests
    ///</summary>
    [TestClass()]
    public class TaskTest
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
        public TaskTest()
        {
            
        }
        protected Project project {get; set; }
         
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
            project = Project.Add(new Project("myproject", "description") { Archived = false });
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
            Task task = new Task("task", "desctask", 10, DateTime.Now, project.Id);
            Task.Add(task);
            Session.SaveChanges();
            Assert.IsFalse(string.IsNullOrEmpty(task.Id));
        }

        /// <summary>
        ///A test for Authenticate
        ///</summary>
        [TestMethod()]
        public void GetTest_successfull_if_properties_are_the_same()
        {
            Task task = new Task("task", "desctask", 10, DateTime.Now, project.Id);
            Task.Add(task);
            Session.SaveChanges();
            Task loaded = Task.FindOne(task.Id);
            Assert.AreEqual(task, loaded);
        }

        /// <summary>
        ///A test for Authenticate
        ///</summary>
        [TestMethod()]
        public void RemoveTest_successfull_if_get_Fails()
        {
            Task task = new Task("task", "desctask", 10, DateTime.Now, project.Id);
            Task.Add(task);
            Session.SaveChanges();
            Task.Remove(task);
            Session.SaveChanges();
            Task loaded = Task.FindOne(task.Id);
            Assert.IsNull(loaded);
        }

        /// <summary>
        ///A test for Load
        ///</summary>
        [TestMethod()]
        public void LoadTest()
        {
            Task task = new Task("task", "desctask", 10, DateTime.Now, project.Id);
            Task.Add(task);
            Session.SaveChanges();
            Task loaded = Task.Load(task.Id);
            Assert.AreEqual(task, loaded);
            
        }
        
        /// <summary>
        ///A test for People
        ///</summary>
        [TestMethod()]
        public void PeopleTest()
        {
            Task task = new Task("task", "desctask", 10, DateTime.Now, project.Id);
            Task.Add(task);
            var person = Person.Add(new Person("email", "pwd", "name"));
            Session.SaveChanges();
            task.PeopleReference.Add(person);
            Session.SaveChanges();

            var loaded = Task.Load(task.Id);

            Assert.IsTrue(loaded.People.Contains(person));
            
        }
        
        /// <summary>
        ///A test for Predecessor
        ///</summary>
        [TestMethod()]
        public void PredecessorTest()
        {
            Task task = new Task("task", "desctask", 10, DateTime.Now, project.Id);
            Task.Add(task);
            Session.SaveChanges();
            var pred = Task.Add(new Task("task", "desctask", 10, DateTime.Now, project.Id));
            task.PredecessorReference.Add(task);
            Session.SaveChanges();

            var loaded = Task.Load(task.Id);

            Assert.IsTrue(loaded.Predecessor.Contains(task));
            
        }
    }
}
