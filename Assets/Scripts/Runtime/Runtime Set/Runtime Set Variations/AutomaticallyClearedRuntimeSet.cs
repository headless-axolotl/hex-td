using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Lotl.Runtime.Generic;
using System;

namespace Lotl.Runtime
{
    public abstract class AutomaticallyClearedRuntimeSet<T> : RuntimeSet<T>
        where T : NotifyOnDestroy
    {
        public override void Add(T item)
        {
            if (item == null) return;
            item.IsBeingDestroyed += HandleItemDestruction;
            base.Add(item);
        }

        public override void Remove(T item)
        {
            if (item == null) return;
            item.IsBeingDestroyed -= HandleItemDestruction;
            base.Remove(item);
        }

        private void HandleItemDestruction(object item, EventArgs _)
        {
            if(item is not T)
            {
                Debug.LogError($"{GetType()} [{name}] received a destruction event with an incorrect sender!" +
                    $"\nSender was: {item}.");
                return;
            }
            Remove(item as T);
        }
    }
}
