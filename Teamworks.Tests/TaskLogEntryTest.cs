using Teamworks.Core.Projects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Teamworks.Core.People;
using Teamworks.Core.Entities;

namespace Teamworks.Tests
{
    
    
    /// <summary>
    ///This is a test class for TaskLogEntryTest and is intended
    ///to contain all TaskLogEntryTest Unit Tests
    ///</summary>
    [TestClass()]
    public class TaskLogEntryTest
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
        ///A test for TaskLogEntry Constructor
        ///</summary>
        [TestMethod()]
        public void TaskLogEntryConstructorTest()
        {
            TaskLogEntry target = new TaskLogEntry();
            Assert.Inconclusive("TODO: Implement code to verify target");
        }

        /// <summary>
        ///A test for Load
        ///</summary>
        [TestMethod()]
        public void LoadTest()
        {
            string id = string.Empty; // TODO: Initialize to an appropriate value
            TaskLogEntry expected = null; // TODO: Initialize to an appropriate value
            TaskLogEntry actual;
            actual = TaskLogEntry.Load(id);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for Date
        ///</summary>
        [TestMethod()]
        public void DateTest()
        {
            TaskLogEntry target = new TaskLogEntry(); // TODO: Initialize to an appropriate value
            DateTime expected = new DateTime(); // TODO: Initialize to an appropriate value
            DateTime actual;
            target.Date = expected;
            actual = target.Date;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for Description
        ///</summary>
        [TestMethod()]
        public void DescriptionTest()
        {
            TaskLogEntry target = new TaskLogEntry(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.Description = expected;
            actual = target.Description;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for Duration
        ///</summary>
        [TestMethod()]
        public void DurationTest()
        {
            TaskLogEntry target = new TaskLogEntry(); // TODO: Initialize to an appropriate value
            long expected = 0; // TODO: Initialize to an appropriate value
            long actual;
            target.Duration = expected;
            actual = target.Duration;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for Owner
        ///</summary>
        [TestMethod()]
        public void OwnerTest()
        {
            TaskLogEntry target = new TaskLogEntry(); // TODO: Initialize to an appropriate value
            Person expected = null; // TODO: Initialize to an appropriate value
            Person actual;
            target.Owner = expected;
            actual = target.Owner;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for OwnerReference
        ///</summary>
        [TestMethod()]
        public void OwnerReferenceTest()
        {
            TaskLogEntry target = new TaskLogEntry(); // TODO: Initialize to an appropriate value
            Reference<Person> expected = null; // TODO: Initialize to an appropriate value
            Reference<Person> actual;
            target.OwnerReference = expected;
            actual = target.OwnerReference;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }
    }
}
