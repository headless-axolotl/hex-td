using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Lotl.StateMachine;
using Lotl.Units.Towers;

namespace Lotl.Units.Generic.StateMachine
{
    [CreateAssetMenu(fileName = "SeekClosestTarget", menuName = "Lotl/Units/Generic/States/Seek/Seek Closest Target")]
    public class SeekClosestTarget : State
    {
        public override void OnEnter(Driver driver)
        {
            if (driver is not ISeeker seeker)
            {
                Debug.LogError($"Driver [{driver.name}] entered incompatible State [{name}]!");
                return;
            }
            seeker.CurrentTarget = null;
        }

        public override void Tick(Driver driver)
        {
            if (driver is not ISeeker seeker) return;
            if (seeker.CurrentTarget != null) return;

            List<Unit> foundUnits = Utility.Scan(
                driver.transform.position,
                seeker.SeekRange,
                seeker.ScanMask,
                seeker.ScanTribeMask);

            Unit targetToSet = null;
            float minDistance = float.MaxValue, distance;
            foreach (Unit unit in foundUnits)
            {
                distance = (unit.transform.position - driver.transform.position).sqrMagnitude;
                if (distance < minDistance)
                {
                    minDistance = distance;
                    targetToSet = unit;
                }
            }
            seeker.CurrentTarget = targetToSet;
        }

        public override void OnExit(Driver driver) { }
    }
}
