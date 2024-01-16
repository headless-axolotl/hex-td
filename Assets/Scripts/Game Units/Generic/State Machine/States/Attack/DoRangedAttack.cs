using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Lotl.StateMachine;

namespace Lotl.Units.Generic.StateMachine
{
    [CreateAssetMenu(fileName = "DoRangedAttack", menuName = "Lotl/Units/Generic/States/Attac/Do Ranged Attack")]
    public class DoRangedAttack : State
    {
        public override void OnEnter(Driver driver)
        {
            if(driver is not IRangedAttacker)
            {
                Debug.LogError($"Driver [{driver.name}] entered incompatible State [{name}]!");
                return;
            }
        }

        public override void Tick(Driver driver)
        {
            if (driver is not IRangedAttacker rangedAttacker) return;

            if (rangedAttacker.TryAttack())
                rangedAttacker.Attack();
        }

        public override void OnExit(Driver driver) { }
    }
}