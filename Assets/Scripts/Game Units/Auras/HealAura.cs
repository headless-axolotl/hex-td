using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Lotl.Generic.Variables;
using Unity.VisualScripting;

namespace Lotl.Units.Aura
{
    public class HealAura : Aura
    {
        [Header("Heal Data")]
        [SerializeField] private FloatReference healCap;
        [SerializeField] private FloatReference healAmountFraction;

        protected override void AuraTick(List<Unit> units)
        {
            bool healed = false;
            foreach (Unit unit in units)
            {
                if (!ShouldHeal(unit)) continue;
                healed = true;
                Heal(unit);
            }

            if (healed) InvokeTickEvent();
        }

        private bool ShouldHeal(Unit unit)
        {
            float healthPercentage = unit.Health / unit.MaxHealth;
            return healthPercentage < healCap;
        }

        private void Heal(Unit unit)
        {
            float amount = unit.MaxHealth * healAmountFraction;
            unit.Heal(amount);
        }
    }
}
