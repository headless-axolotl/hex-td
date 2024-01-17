using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Lotl.StateMachine;
using Lotl.Units.Attackers;

namespace Lotl.Units.Generic.StateMachine
{
    [CreateAssetMenu(fileName = "MoveTowardsTarget", menuName = "Lotl/Units/Generic/States/Seek/Move Towards Target")]
    public class MoveTowardsTarget : State
    {
        public override void OnEnter(Driver driver)
        {
            if (driver is not IMobileSeeker mobileSeeker)
            {
                Debug.LogError($"Driver [{driver.name}] entered incompatible State [{name}]!");
                return;
            }
            mobileSeeker.Locomotion.StopMoving();
        }

        public override void Tick(Driver driver)
        {
            if (driver is not IMobileSeeker mobileSeeker) return;

            mobileSeeker.UpdateMovement();
        }

        public override void OnExit(Driver driver)
        {
            if (driver is not IMobileSeeker mobileSeeker) return;

            mobileSeeker.Locomotion.StopMoving();
        }
    }
}