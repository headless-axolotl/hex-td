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
        protected HashSet<T> items = new();
        public IReadOnlyCollection<T> Items => items;

        public override void Clear()
        {
            items.Clear();
        }

        public virtual void Add(T item)
        {
            items.Add(item);
        }

        public virtual void Remove(T item)
        {
            items.Remove(item);
        }

        public bool Contains(T item)
        {
            return items.Contains(item);
        }

        public override int Count() => items.Count;
    }
}
