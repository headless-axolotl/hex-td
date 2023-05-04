using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lotl.Runtime
{
    public abstract class RuntimeSet<T> : ScriptableObject
    {
        private HashSet<T> items = new();
        public IReadOnlyCollection<T> Items => items;

        public virtual void Initialize()
        {
            items.Clear();
        }

        public void Add(T item)
        {
            items.Add(item);
        }

        public void Remove(T item)
        {
            items.Remove(item);
        }

        public bool Contains(T item)
        {
            return items.Contains(item);
        }
    }
}
