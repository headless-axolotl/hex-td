using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Lotl.Units.Damage
{
    [RequireComponent(typeof(Unit))]
    public abstract class DamageTriggerHandler : MonoBehaviour
    {
        [SerializeField] private DamageTrigger[] triggersToHandler;

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

            bool shouldRespond = damageInfo.Triggers.Any(trigger => triggersToHandler.Contains(trigger));
            if (shouldRespond) RespondToTrigger(damageInfo);
        }

        protected abstract void RespondToTrigger(DamageInfo damageInfo);
    }
}
