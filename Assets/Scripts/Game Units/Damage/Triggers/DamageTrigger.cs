using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Lotl.Generic.Variables;

namespace Lotl.Units.Damage
{
    [CreateAssetMenu(fileName = "DamageTrigger", menuName = "Lotl/Units/Damage/Damage Trigger")]
    public class DamageTrigger : ScriptableObject
    {
        [SerializeField] private Identity triggerType;
        [SerializeField] private List<IdentityValue> identityValues;

        public Identity TriggerType => triggerType;

        public T GetValue<T>(Identity identity)
        {
            IdentityValue identityValue = null;
            for(int i = 0; i < identityValues.Count; i++)
            {
                if (identityValues[i].Type == identity)
                {
                    identityValue = identityValues[i];
                    break;
                }
            }
            if (identityValue == null) return default;
            
            if (identityValue.Value is not ScriptableObjectVariable<T> variable) return default;
            
            return variable.Value;
        }

        [System.Serializable]
        public class IdentityValue
        {
            [SerializeField] private Identity type;
            [SerializeField] private ScriptableObject value;

            public Identity Type => type;
            public ScriptableObject Value => value;
        }
    }
}

