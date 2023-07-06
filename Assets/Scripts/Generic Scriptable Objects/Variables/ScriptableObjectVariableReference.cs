using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lotl.Generic.Variables
{
    public abstract class ScriptableObjectVariableReferenceBase { }

    public abstract class ScriptableObjectVariableReference<T, TVariable> : ScriptableObjectVariableReferenceBase
        where TVariable : ScriptableObjectVariable<T>
    {
        [SerializeField] private bool useConstant = true;
        [SerializeField] private T constantValue;
        [SerializeField] private TVariable variable;

        public ScriptableObjectVariableReference()
        { }

        public ScriptableObjectVariableReference(T value)
        {
            useConstant = true;
            constantValue = value;
        }

        public T Value
        {
            get { return useConstant ? constantValue : variable.Value; }
        }

        public static implicit operator T(ScriptableObjectVariableReference<T, TVariable> reference)
            => reference.Value;
    }
}
