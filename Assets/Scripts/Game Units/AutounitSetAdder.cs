using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Lotl.Runtime;
using Lotl.Runtime.Generic;

namespace Lotl.Units.Towers
{
    public class AutounitSetAdder : NotifyOnDestroy
    {
        [SerializeField] protected AutounitRuntimeSet runtimeSet;

        private void Awake()
        {
            if (runtimeSet == null)
            {
                Debug.LogError($"Tower [{name}] is missing a runtime set reference!");
                return;
            }
            runtimeSet.Add(this);
        }
    }
}
