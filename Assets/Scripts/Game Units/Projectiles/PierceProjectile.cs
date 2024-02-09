using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Lotl.Generic.Variables;

namespace Lotl.Units.Projectiles
{
    public class PierceProjectile : Projectile
    {
        [Header("Additional Data")]
        [SerializeField] private Identity pierceCountId;

        private HashSet<Unit> currentHitUnits = new();
        private int currentPierceCount;

        public override void Initialize(ProjectileInfo projetileInfo)
        {
            base.Initialize(projetileInfo);

            currentHitUnits.Clear();
            currentPierceCount = currentConfiguration.GetValue<int>(pierceCountId);
        }

        protected override void DealDamage(List<Unit> units)
        {
            Unit unitToDamage = null;
            for(int i = 0; i < units.Count; i++)
            {
                if (!currentHitUnits.Contains(units[i]))
                {
                    unitToDamage = units[i];
                    break;
                }
            }

            if (unitToDamage == null) return;

            DealDamage(unitToDamage);

            currentHitUnits.Add(unitToDamage);
            currentPierceCount--;

            if (currentPierceCount == 0)
                gameObject.SetActive(false);
        }
    }
}