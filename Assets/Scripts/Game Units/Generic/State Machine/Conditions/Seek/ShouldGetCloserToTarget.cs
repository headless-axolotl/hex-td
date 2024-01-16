using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Lotl.StateMachine;

namespace Lotl.Units.Generic.StateMachine
{
    [CreateAssetMenu(fileName = "ShouldMoveCloserToTarget",
        menuName = "Lotl/Units/Generic/Conditions/Target/Should Move Closer To Target")]
    public class ShouldGetCloserToTarget : Condition
    {
        [SerializeField] private bool invert = false;

        public override bool IsMet(Driver driver)
        {
            if (driver is not IMobileSeeker seeker)
            {
                Debug.LogError($"Driver [{driver.name}] entered incompatible State [{name}]!");
                return false;
            }

            if (seeker.CurrentTarget == null) return false;

            bool result = seeker.ShouldGetCloserToTarget();

            if (!invert) return result;
            return !result;
        }
    }
}