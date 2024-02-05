using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Lotl.Generic.Variables;

namespace Lotl.Units.Damage
{
    [RequireComponent(typeof(Unit))]
    public abstract class DamageTriggerHandler : MonoBehaviour
    {
        [SerializeField] private Identity[] triggersToHandle;

        protected Unit unit;

        private void Awake()
        {
            unit = GetComponent<Unit>();
        }

        private void OnEnable()
        {
            unit.WasDamaged += HandleDamageTrigger;
        }

        private void OnDisable()
        {
            unit.WasDamaged -= HandleDamageTrigger;
        }

        private void HandleDamageTrigger(DamageInfo damageInfo)
        {
            if (damageInfo.Triggers == null) return;

            DamageTrigger matchedTrigger = null;
            
            for(int i = 0; i < damageInfo.Triggers.Length; i++)
            {
                for(int j = 0; j < triggersToHandle.Length; j++)
                {
                    if (triggersToHandle[j] == damageInfo.Triggers[i].TriggerType)
                    {
                        matchedTrigger = damageInfo.Triggers[i];
                        break;
                    }
                }
            }

            if (matchedTrigger == null) return;

            RespondToTrigger(damageInfo, matchedTrigger);
        }

        protected abstract void RespondToTrigger(DamageInfo damageInfo, DamageTrigger trigger);
    }
}
