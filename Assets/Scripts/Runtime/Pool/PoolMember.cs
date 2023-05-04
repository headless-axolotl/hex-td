using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lotl.Runtime
{
    public class PoolMember : MonoBehaviour
    {
        public event EventHandler OnDeactivate;

        public event EventHandler IsBeingDestroyed;

        private void OnDisable()
        {
            OnDeactivate?.Invoke(this, null);
        }

        private void OnDestroy()
        {
            IsBeingDestroyed?.Invoke(this, null);
        }
    }
}
