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
        ///A test for Project Constructor
        ///</summary>
        [TestMethod()]
        public void ProjectConstructorTest()
        {
            Project target = new Project();
            Assert.Inconclusive("TODO: Implement code to verify target");
        }

        /// <summary>
        ///A test for Load
        ///</summary>
        [TestMethod()]
        public void LoadTest()
        {
            string id = string.Empty; // TODO: Initialize to an appropriate value
            Project expected = null; // TODO: Initialize to an appropriate value
            Project actual;
            actual = Project.Load(id);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for Archived
        ///</summary>
        [TestMethod()]
        public void ArchivedTest()
        {
            Project target = new Project(); // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            target.Archived = expected;
            actual = target.Archived;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for Description
        ///</summary>
        [TestMethod()]
        public void DescriptionTest()
        {
            Project target = new Project(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.Description = expected;
            actual = target.Description;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for People
        ///</summary>
        [TestMethod()]
        public void PeopleTest()
        {
            Project target = new Project(); // TODO: Initialize to an appropriate value
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
            Project target = new Project(); // TODO: Initialize to an appropriate value
            IList<Reference<Person>> expected = null; // TODO: Initialize to an appropriate value
            IList<Reference<Person>> actual;
            target.PeopleReference = expected;
            actual = target.PeopleReference;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for Tasks
        ///</summary>
        [TestMethod()]
        public void TasksTest()
        {
            Project target = new Project(); // TODO: Initialize to an appropriate value
            IList<Task> expected = null; // TODO: Initialize to an appropriate value
            IList<Task> actual;
            target.Tasks = expected;
            actual = target.Tasks;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for TasksReference
        ///</summary>
        [TestMethod()]
        public void TasksReferenceTest()
        {
            Project target = new Project(); // TODO: Initialize to an appropriate value
            IList<Reference<Task>> expected = null; // TODO: Initialize to an appropriate value
            IList<Reference<Task>> actual;
            target.TasksReference = expected;
            actual = target.TasksReference;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for TotalConsumedHours
        ///</summary>
        [TestMethod()]
        public void TotalConsumedHoursTest()
        {
            Project target = new Project(); // TODO: Initialize to an appropriate value
            long actual;
            actual = target.TotalConsumedHours;
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for TotalEstimatedHours
        ///</summary>
        [TestMethod()]
        public void TotalEstimatedHoursTest()
        {
            Project target = new Project(); // TODO: Initialize to an appropriate value
            long actual;
            actual = target.TotalEstimatedHours;
            Assert.Inconclusive("Verify the correctness of this test method.");
        }
    }
}
