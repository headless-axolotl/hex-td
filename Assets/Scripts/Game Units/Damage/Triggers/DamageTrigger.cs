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
        [SerializeField] private IdentityDictionary dictionary;

        public Identity TriggerType => triggerType;

        public IdentityDictionary Dictionary => dictionary;
    }
}

