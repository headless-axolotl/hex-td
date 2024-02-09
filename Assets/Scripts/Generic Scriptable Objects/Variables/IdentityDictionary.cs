using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lotl.Generic.Variables
{
    [System.Serializable]
    public class IdentityDictionary
    {
        [SerializeField] private List<IdentityValue> values = new();
        
        public T GetValue<T>(Identity identity)
        {
            IdentityValue identityValue = null;
            for (int i = 0; i < values.Count; i++)
            {
                if (values[i].Identity == identity)
                {
                    identityValue = values[i];
                    break;
                }
            }
            
            if (identityValue == null)
                return default;
            
            if (identityValue.Value is not ScriptableObjectVariable<T> variable)
                return default;

            return variable.Value;
        }
    }

    [System.Serializable]
    public class IdentityValue
    {
        [SerializeField] private Identity identity;
        [SerializeField] private ScriptableObject value;

        public Identity Identity => identity;
        public ScriptableObject Value => value;
    }
}
