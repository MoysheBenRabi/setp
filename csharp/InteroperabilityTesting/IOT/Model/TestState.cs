using System;
using System.Collections.Generic;
using System.Text;

namespace IOT.Model
{
    /// <summary>
    /// State of IOT suite test.
    /// </summary>
    public class TestState
    {
        /// <summary>
        /// Key of the test.
        /// </summary>
        public TestKey Key;
        /// <summary>
        /// Category of the test.
        /// </summary>
        public TestCategory Category;
        /// <summary>
        /// True if test has been executed succesfully.
        /// </summary>
        public Nullable<bool> Result;
        /// <summary>
        /// Error message describing why test failed.
        /// </summary>
        public string ErrorMessage;
    }
}
