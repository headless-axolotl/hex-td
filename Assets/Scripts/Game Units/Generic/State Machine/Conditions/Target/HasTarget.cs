using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Lotl.StateMachine;

namespace Lotl.Units.Generic.StateMachine
{
    [CreateAssetMenu(fileName = "HasTarget", menuName = "Lotl/Units/Generic/Conditions/Target/Has Target")]
    public class HasTarget : Condition
    {
        [SerializeField] private bool invert = false;

        public override bool IsMet(Driver driver)
        {
            if (driver is not IContainsTarget containsTarget)
            {
                Debug.LogError($"Driver [{driver.name}] used incpomatible Condition [{name}]!");
                return false;
            }
            bool result = containsTarget.CurrentTarget != null;

            if (!invert) return result;
            return !result;
        }
    }
}
