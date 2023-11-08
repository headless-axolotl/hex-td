using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Lotl.Utility;

namespace Lotl.Data
{
    public class BaseManager : Initiable
    {
        protected bool ProperlyInitialized(Action<bool> callback)
        {
            if (!IsInitialized)
            {
                callback?.Invoke(false);
                return false;
            }
            return true;
        }

        protected bool CheckInitialization<T>(Action<T, bool> callback)
        {
            if (!IsInitialized)
            {
                callback?.Invoke(default, false);
                return false;
            }
            return true;
        }
    }
}
