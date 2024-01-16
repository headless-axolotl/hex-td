using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Lotl.Units.Damage;
using Lotl.Runtime.Generic;

namespace Lotl.Units.Generic.StateMachine
{
    public interface IAttacker : ISeeker, IContainsPosition
    {
        float AttackRange { get; }

        Timer Timer { get; }
        float ActionCooldown { get; }
        float ActionDelay { get; }
        ActionState State { get; set; }

        bool TryAttack()
        {
            if (!IsCurrentTargetValid()) return false;

            if (Timer.IsTicking) return false;

            if (State == ActionState.Cooldown)
            {
                Timer.Trigger(ActionDelay);
                State = ActionState.Delay;
                TriggerAttackEvent();
                return false;
            }
            else
            {
                Timer.Trigger(ActionCooldown);
                State = ActionState.Cooldown;
            }

            return true;
        }

        bool IsCurrentTargetValid()
        {
            if (CurrentTarget == null) return false;
            float sqrDistance = (CurrentPosition - CurrentTarget.transform.position).sqrMagnitude;

            if (sqrDistance > AttackRange * AttackRange)
            {
                CurrentTarget = null;
                return false;
            }
            return true;
        }

        void TriggerAttackEvent();
    }
}
