using Lotl.Runtime.Generic;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lotl.Runtime
{
    public class PoolMember : NotifyOnDestroy
    {
        public event Action<PoolMember> OnDeactivate;

        private void OnDisable()
        {
            OnDeactivate?.Invoke(this);
        }
    }
}
