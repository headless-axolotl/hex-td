using Lotl.Units.Locomotion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lotl.Units.Generic.StateMachine
{
    public interface IMobileSeeker : ISeeker, IContainsPosition
    {
        float Reach { get; }

        MobileUnitLocomotion Locomotion { get; }

        void UpdateMovement()
        {
            if (ShouldGetCloserToTarget())
            {
                Locomotion.MoveTowards(CurrentTarget.transform.position);
                return;
            }

            Locomotion.StopMoving();
        }

        bool ShouldGetCloserToTarget()
        {
            float sqrDistance = (CurrentPosition - CurrentTarget.transform.position).sqrMagnitude;
            if (sqrDistance > Reach * Reach) return true;
            return false;
        }
    }
}