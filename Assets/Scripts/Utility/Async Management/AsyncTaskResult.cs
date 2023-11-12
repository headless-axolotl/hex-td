using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lotl.Utility.Async
{
    public class AsyncTaskResult
    {
        private readonly bool wasSuccessful;
        private readonly string message;

        public bool WasSuccessful => wasSuccessful;
        public string Message => message;

        public AsyncTaskResult(bool wasSuccessful, string message)
        {
            this.wasSuccessful = wasSuccessful;
            this.message = message;
        }

        public static readonly AsyncTaskResult OK = new(true, string.Empty);
    }
}
