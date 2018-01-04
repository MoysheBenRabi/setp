using System;
using System.Collections.Generic;
using System.Web;
using MXP;
using IOT.Encoding;

namespace IOT.Model
{
    /// <summary>
    /// Contains states of the test suit tests.
    /// </summary>
    public class TestSuiteState
    {
        /// <summary>
        /// Reference server log
        /// </summary>
        public string ReferenceServerLog;

        /// <summary>
        /// Test states by category.
        /// </summary>
        private IDictionary<TestCategory, IDictionary<TestKey, TestState>> TestStates;
        /// <summary>
        /// Message test states.
        /// </summary>
        public IDictionary<string, MessageTestState> MessageTestStates;

        /// <summary>
        /// Default construct which initializes the report.
        /// </summary>
        public TestSuiteState()
        {

            IDictionary<string, MessageTestState> messageTestStates = new Dictionary<string, MessageTestState>();

            foreach (ReferenceMessage referenceMessage in ReferenceMessageLoader.Current.ReferenceMessages.Values)
            {
                MessageTestState item = new MessageTestState();
                item.MessageName = referenceMessage.MessageName;
                item.MessageFileName = referenceMessage.MessageFileName;
                item.ReferenceMessage = referenceMessage.MessageValue;
                item.ReferenceString = referenceMessage.StringValue;
                item.ReferenceBytes = referenceMessage.ByteValue;
                messageTestStates.Add(item.MessageFileName, item);
            }

            MessageTestStates = messageTestStates;

            TestStates = new Dictionary<TestCategory, IDictionary<TestKey, TestState>>();
            TestStates.Add(TestCategory.MessageSerialization, new Dictionary<TestKey, TestState>());
            TestStates.Add(TestCategory.CandidateClientToReferenceServer, new Dictionary<TestKey, TestState>());
            TestStates.Add(TestCategory.ReferenceClientToCandidateServer, new Dictionary<TestKey, TestState>());
            TestStates.Add(TestCategory.CandidateServerToReferenceServer, new Dictionary<TestKey, TestState>());
            TestStates.Add(TestCategory.ReferenceServerToCandidateServer, new Dictionary<TestKey, TestState>());

            InitializeTestState(TestCategory.CandidateClientToReferenceServer, TestKey.Connection);
            InitializeTestState(TestCategory.CandidateClientToReferenceServer, TestKey.ObjectInjection);
            InitializeTestState(TestCategory.CandidateClientToReferenceServer, TestKey.ObjectExamination);
            InitializeTestState(TestCategory.CandidateClientToReferenceServer, TestKey.ObjectModification);
            InitializeTestState(TestCategory.CandidateClientToReferenceServer, TestKey.ObjectInteraction);
            InitializeTestState(TestCategory.CandidateClientToReferenceServer, TestKey.ObjectEjection);
            InitializeTestState(TestCategory.CandidateClientToReferenceServer, TestKey.Disconnection);
        }

        /// <summary>
        /// Initializes state of test to report.
        /// </summary>
        /// <param name="category">Category where the test belongs to.</param>
        /// <param name="key">Key of the test.</param>
        private void InitializeTestState(TestCategory category, TestKey key)
        {
            lock (TestStates)
            {
                TestState testState = new TestState();
                testState.Category = category;
                testState.Key = key;
                TestStates[testState.Category].Add(testState.Key, testState);
            }
        }
        /// <summary>
        /// Marks test as successfull.
        /// </summary>
        /// <param name="category">Category of the test</param>
        /// <param name="testKey">Key of the test</param>
        public void MarkTestSuccess(TestCategory category, TestKey testKey)
        {
            lock (TestStates)
            {
                TestStates[category][testKey].Result = true;
                TestStates[category][testKey].ErrorMessage = "";
            }
        }
        /// <summary>
        /// Marks test as failed with given error message.
        /// </summary>
        /// <param name="category">Category of the test</param>
        /// <param name="testKey">Key of the test</param>
        /// <param name="errorMessage">Message describing why test failed.</param>
        public void MarkTestFailure(TestCategory category, TestKey testKey, string errorMessage)
        {
            lock (TestStates)
            {
                TestStates[category][testKey].Result = false;
                TestStates[category][testKey].ErrorMessage = errorMessage;
            }
        }
        /// <summary>
        /// Returns test states in a given category.
        /// </summary>
        /// <param name="category">The test cateogry of interest</param>
        /// <returns>List of test states</returns>
        public IList<TestState> GetTestStates(TestCategory category)
        {
            lock (TestStates)
            {
                List<TestState> testStateList = new List<TestState>();
                foreach (TestState testState in TestStates[category].Values)
                {
                    testStateList.Add(testState);
                }
                return testStateList;
            }
        }

        /// <summary>
        /// Returns number of tests for given test category.
        /// </summary>
        /// <param name="category">The test category of interest.</param>
        /// <returns>Maximum score</returns>
        public int GetCategoryTestCount(TestCategory category)
        {
            if (category == TestCategory.MessageSerialization)
            {
                return MessageTestStates.Values.Count;
            }

            return TestStates[category].Values.Count;
        }
        /// <summary>
        /// Returns number of passed tests for given test category.
        /// </summary>
        /// <param name="category">The test category of interest.</param>
        /// <returns>The score</returns>
        public int GetCategoryPassedCount(TestCategory category)
        {
            if (category == TestCategory.MessageSerialization)
            {
                int count = 0;
                foreach (MessageTestState testState in MessageTestStates.Values)
                {
                    if (testState.Result == true)
                    {
                        count++;
                    }
                }
                return count;
            }
            else
            {
                int count = 0;
                foreach (TestState testState in TestStates[category].Values)
                {
                    if (testState.Result == true)
                    {
                        count++;
                    }
                }
                return count;
            }
        }
        /// <summary>
        /// Returns number of failed tests for given test category.
        /// </summary>
        /// <param name="category">The test category of interest.</param>
        /// <returns>The score</returns>
        public int GetCategoryFailedCount(TestCategory category)
        {
            if (category == TestCategory.MessageSerialization)
            {
                int count = 0;
                foreach (MessageTestState testState in MessageTestStates.Values)
                {
                    if (testState.Result == false)
                    {
                        count++;
                    }
                }
                return count;
            }
            else
            {
                int count = 0;
                foreach (TestState testState in TestStates[category].Values)
                {
                    if (testState.Result == false)
                    {
                        count++;
                    }
                }
                return count;
            }
        }

    }
}
