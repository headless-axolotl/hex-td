using Lotl.Units.Locomotion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lotl.Units.Generic.StateMachine
{
    public interface IMobileSeeker : ISeeker, IContainsPosition
    {
        float Range { get; }

        UnitLocomotion Locomotion { get; }

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
            if (sqrDistance > Range * Range) return true;
            return false;
        }
    }
}