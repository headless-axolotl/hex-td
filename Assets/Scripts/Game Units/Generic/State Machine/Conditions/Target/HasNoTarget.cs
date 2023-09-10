using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Lotl.StateMachine;

namespace Lotl.Units.Generic.StateMachine
{
    [CreateAssetMenu(fileName = "HasNoTarget", menuName = "Lotl/Units/Generic/Conditions/Target/Has No Target")]
    public class HasNoTarget : Condition
    {
        public override bool IsMet(Driver driver)
        {
            if (driver is not IContainsTarget containsTarget)
            {
                Debug.LogError($"Driver [{driver.name}] used incpomatible Condition [{name}]!");
                return false;
            }
            return containsTarget.CurrentTarget == null;
        }
    }
}
