using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Lotl.StateMachine;

namespace Lotl.Units.Generic.StateMachine
{
    [CreateAssetMenu(fileName = "EmptyState", menuName = "Lotl/Units/Generic/States/Empty")]
    public class EmptyState : State
    {
        public override void OnEnter(Driver driver) { }
        public override void OnExit(Driver driver) { }
        public override void Tick(Driver driver) { }
    }
}
