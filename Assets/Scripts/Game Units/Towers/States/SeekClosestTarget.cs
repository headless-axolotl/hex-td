using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Lotl.StateMachine;

namespace Lotl.Units.Towers.States
{
    [CreateAssetMenu(fileName = "SeekClosestTarget", menuName = "Lotl/Units/Towers/Projectile Class/States/Seek Closest Target")]
    public class SeekClosestTarget : State
    {
        public override void OnEnter(Driver driver)
        {
            if (driver is not ProjectileClassTower tower)
            {
                Debug.LogError($"Driver [{driver.name}] entered incompatible State [{name}]!");
                return;
            }
            tower.CurrentTarget = null;
        }

        public override void Tick(Driver driver)
        {
            if (driver is not ProjectileClassTower tower) return;
            if (tower.CurrentTarget != null) return;

            List<Unit> foundUnits = Utility.Scan(
                tower.transform.position,
                tower.Range,
                tower.ScanMask,
                tower.ScanTribeMask);

            Unit targetToSet = null;
            float minDistance = float.MaxValue, distance;
            foreach (Unit unit in foundUnits)
            {
                distance = (unit.transform.position - tower.transform.position).sqrMagnitude;
                if (distance < minDistance)
                {
                    distance = minDistance;
                    targetToSet = unit;
                }
            }
            tower.CurrentTarget = targetToSet;
        }

        public override void OnExit(Driver driver) { }
    }
}

