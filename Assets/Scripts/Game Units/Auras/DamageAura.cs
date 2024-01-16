using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Lotl.Generic.Variables;
using Lotl.Units.Damage;

namespace Lotl.Units.Aura
{
    public class DamageAura : Aura
    {
        [Header("Damage Data")]
        [SerializeField] private FloatReference damage;
        [SerializeField] private DamageTrigger[] triggers;

        protected override void AuraTick(List<Unit> units)
        {
            if (units.Count == 0) return;

            foreach(Unit unit in units)
            {
                unit.TakeDamage(damage, transform.position, triggers);
            }

            InvokeTickEvent();
        }
    }
}