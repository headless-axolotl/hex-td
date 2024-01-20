using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Lotl.Generic.Variables
{
    public abstract class ScriptableObjectVariable<T> : ScriptableObject
    {
        public event Action<T> Changed;
        
        [SerializeField] private T value;
        
        public T Value
        {
            get => value;
            set
            {
                this.value = value;
                Changed?.Invoke(this.value);
            }
        }
    }
}
