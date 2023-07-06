using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Lotl.StateMachine;

namespace Lotl.Units.Generic.StateMachine
{
    [CreateAssetMenu(fileName = "AlwaysTrue", menuName = "Units/Generic/Conditions/Always True")]
    public class AlwaysTrue : Condition
    {
        public override bool IsMet(Driver driver) => true;
    }
}

