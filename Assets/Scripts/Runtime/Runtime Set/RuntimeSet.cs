using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lotl.Runtime
{
    public abstract class RuntimeSetBase: ScriptableObject
    {
        public abstract void Clear();
        public abstract int Count();
    }

    public abstract class RuntimeSet<T> : RuntimeSetBase
    {
        public event Action<RuntimeSet<T>> Changed;

        protected HashSet<T> items = new();
        public IReadOnlyCollection<T> Items => items;

        public override void Clear()
        {
            items.Clear();
            Changed?.Invoke(this);
        }

        public virtual void Add(T item)
        {
            items.Add(item);
            Changed?.Invoke(this);
        }

        public virtual void Remove(T item)
        {
            items.Remove(item);
            Changed?.Invoke(this);
        }

        public bool Contains(T item)
        {
            return items.Contains(item);
        }

        public override int Count() => items.Count;
    }
}
