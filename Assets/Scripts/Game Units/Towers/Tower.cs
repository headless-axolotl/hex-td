using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Lotl.Runtime;
using Lotl.Runtime.Generic;

namespace Lotl.Units.Towers
{
    [RequireComponent(typeof(Unit))]
    public class Tower : NotifyOnDestroy
    {
        [SerializeField] protected TowerRuntimeSet runtimeSet;

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
