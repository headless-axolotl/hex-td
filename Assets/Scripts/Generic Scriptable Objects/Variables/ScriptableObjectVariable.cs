using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Lotl.Generic.Variables
{
    public abstract class ScriptableObjectVariable<T> : ScriptableObject
    {
        public T Value;
    }
}
