using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lotl.Runtime.Generic
{
    public class NotifyOnDestroy : MonoBehaviour
    {
        public event EventHandler IsBeingDestroyed;

        private void OnDestroy()
        {
            IsBeingDestroyed?.Invoke(this, null);
        }
    }
}
