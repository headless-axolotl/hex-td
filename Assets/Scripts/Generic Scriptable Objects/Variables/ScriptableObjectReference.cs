using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lotl.Generic.Variables
{
    public abstract class ScriptableObjectReferenceBase { }

    public abstract class ScriptableObjectReference<T, TVariable> : ScriptableObjectReferenceBase
        where TVariable : ScriptableObjectVariable<T>
    {
        [SerializeField] private bool useConstant = true;
        [SerializeField] private T constantValue;
        [SerializeField] private TVariable variable;

        public ScriptableObjectReference()
        { }

        public ScriptableObjectReference(T value)
        {
            useConstant = true;
            constantValue = value;
        }

        public T Value
        {
            get { return useConstant ? constantValue : variable.Value; }
        }

        public static implicit operator T(ScriptableObjectReference<T, TVariable> reference)
            => reference.Value;
    }
}
