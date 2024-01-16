using Lotl.StateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lotl.Units.Generic.StateMachine
{
    [CreateAssetMenu(fileName = "DoMeleeAttack", menuName = "Lotl/Units/Generic/States/Attac/Do Melee Attack")]
    public class DoMeleeAttack : State
    {
        public override void OnEnter(Driver driver)
        {
            if (driver is not IMeleeAttacker)
            {
                Debug.LogError($"Driver [{driver.name}] entered incompatible State [{name}]!");
                return;
            }
        }

        public override void Tick(Driver driver)
        {
            if (driver is not IMeleeAttacker rangedAttacker) return;

            if (rangedAttacker.TryAttack())
                rangedAttacker.Attack();
        }

        public override void OnExit(Driver driver) { }
    }
}