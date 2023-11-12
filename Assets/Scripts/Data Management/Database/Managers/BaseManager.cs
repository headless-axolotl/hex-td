using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Lotl.Utility;

namespace Lotl.Data
{
    using Result = Utility.Async.AsyncTaskResult;

    public class BaseManager : Initiable
    {
        private const string UnitializedManager = "uninitialized manager";

        protected bool ProperlyInitialized(Action<Result> callback)
        {
            if (!IsInitialized)
            {
                callback?.Invoke(new(false, UnitializedManager));
                return false;
            }
            return true;
        }

        protected bool ProperlyInitialized<T>(Action<Result, T> callback)
        {
            if (!IsInitialized)
            {
                callback?.Invoke(new(false, UnitializedManager), default);
                return false;
            }
            return true;
        }
    }
}
