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
        ///A test for Task Constructor
        ///</summary>
        [TestMethod()]
        public void TaskConstructorTest()
        {
            Task target = new Task();
            Assert.Inconclusive("TODO: Implement code to verify target");
        }

        /// <summary>
        ///A test for Load
        ///</summary>
        [TestMethod()]
        public void LoadTest()
        {
            string id = string.Empty; // TODO: Initialize to an appropriate value
            Task expected = null; // TODO: Initialize to an appropriate value
            Task actual;
            actual = Task.Load(id);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for Consumed
        ///</summary>
        [TestMethod()]
        public void ConsumedTest()
        {
            Task target = new Task(); // TODO: Initialize to an appropriate value
            long expected = 0; // TODO: Initialize to an appropriate value
            long actual;
            target.Consumed = expected;
            actual = target.Consumed;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for Description
        ///</summary>
        [TestMethod()]
        public void DescriptionTest()
        {
            Task target = new Task(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.Description = expected;
            actual = target.Description;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for Due
        ///</summary>
        [TestMethod()]
        public void DueTest()
        {
            Task target = new Task(); // TODO: Initialize to an appropriate value
            DateTime expected = new DateTime(); // TODO: Initialize to an appropriate value
            DateTime actual;
            target.Due = expected;
            actual = target.Due;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for Estimated
        ///</summary>
        [TestMethod()]
        public void EstimatedTest()
        {
            Task target = new Task(); // TODO: Initialize to an appropriate value
            long expected = 0; // TODO: Initialize to an appropriate value
            long actual;
            target.Estimated = expected;
            actual = target.Estimated;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for Log
        ///</summary>
        [TestMethod()]
        public void LogTest()
        {
            Task target = new Task(); // TODO: Initialize to an appropriate value
            IList<TaskLogEntry> expected = null; // TODO: Initialize to an appropriate value
            IList<TaskLogEntry> actual;
            target.Log = expected;
            actual = target.Log;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for People
        ///</summary>
        [TestMethod()]
        public void PeopleTest()
        {
            Task target = new Task(); // TODO: Initialize to an appropriate value
            IList<Person> expected = null; // TODO: Initialize to an appropriate value
            IList<Person> actual;
            target.People = expected;
            actual = target.People;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for PeopleReference
        ///</summary>
        [TestMethod()]
        public void PeopleReferenceTest()
        {
            Task target = new Task(); // TODO: Initialize to an appropriate value
            IList<Reference<Person>> expected = null; // TODO: Initialize to an appropriate value
            IList<Reference<Person>> actual;
            target.PeopleReference = expected;
            actual = target.PeopleReference;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for Predecessor
        ///</summary>
        [TestMethod()]
        public void PredecessorTest()
        {
            Task target = new Task(); // TODO: Initialize to an appropriate value
            IList<Task> expected = null; // TODO: Initialize to an appropriate value
            IList<Task> actual;
            target.Predecessor = expected;
            actual = target.Predecessor;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for PredecessorReference
        ///</summary>
        [TestMethod()]
        public void PredecessorReferenceTest()
        {
            Task target = new Task(); // TODO: Initialize to an appropriate value
            IList<Reference<Task>> expected = null; // TODO: Initialize to an appropriate value
            IList<Reference<Task>> actual;
            target.PredecessorReference = expected;
            actual = target.PredecessorReference;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for Project
        ///</summary>
        [TestMethod()]
        public void ProjectTest()
        {
            Task target = new Task(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.Project = expected;
            actual = target.Project;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for Status
        ///</summary>
        [TestMethod()]
        public void StatusTest()
        {
            Task target = new Task(); // TODO: Initialize to an appropriate value
            Task.TaskStatus expected = new Task.TaskStatus(); // TODO: Initialize to an appropriate value
            Task.TaskStatus actual;
            target.Status = expected;
            actual = target.Status;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }
    }
}
