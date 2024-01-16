using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using Lotl.Generic.Variables;

namespace Lotl.Units.Projectiles
{
    public class LightningProjectile : Projectile
    {
        [Header("Additional Data")]
        [SerializeField] private FloatReference jumpRadius;
        [SerializeField] private IntReference maxJumpCount;

        private HashSet<Unit> currentHitUnits = new();
        private int currentJumpCount = 0;

        public override void Initialize(ProjectileInfo projetileInfo)
        {
            base.Initialize(projetileInfo);
            currentHitUnits.Clear();
            currentJumpCount = maxJumpCount;
        }

        protected override void DealDamage(List<Unit> units)
        {
            Unit unitToDamage = units.FirstOrDefault(unit => !currentHitUnits.Contains(unit));
            
            if(unitToDamage == null)
            {
                currentJumpCount = 0;
                gameObject.SetActive(false);
                return;
            }

            DealDamage(unitToDamage);
            currentHitUnits.Add(unitToDamage);
            currentJumpCount--;

            timer.Stop();
            timer.Trigger(lifetime);

            if (currentJumpCount == 0)
            {
                gameObject.SetActive(false);
                return;
            }

            Unit nextUnit = FindClosestNotHitUnit();
            
            if(nextUnit == null)
            {
                currentJumpCount = 0;
                gameObject.SetActive(false);
                return;
            }

            AlignTo(nextUnit.transform.position);
        }

        private Unit FindClosestNotHitUnit()
        {
            List<Unit> units = Scanner.Scan(
                transform.position, jumpRadius,
                scanLayer, scanTribeMask);
            
            if (units.Count == 0)
                return null;

            float minDistance = float.MaxValue;
            Unit closestUnit = null;
            
            foreach(Unit unit in units)
            {
                if(currentHitUnits.Contains(unit))
                {
                    continue;
                }

                float distance = (transform.position - unit.transform.position).sqrMagnitude;
                
                if(distance < minDistance)
                {
                    minDistance = distance;
                    closestUnit = unit;
                }
            }
            
            return closestUnit;
        }
    }
}