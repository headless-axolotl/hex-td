using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Lotl.Units.Damage;
using Lotl.Runtime.Generic;
using Lotl.Units.Locomotion;

namespace Lotl.Units.Generic.StateMachine
{
    public interface IMeleeAttacker : IAttacker
    {
        float Damage { get; }
        DamageTrigger[] DamageTriggers { get; }

        void Attack()
        {
            CurrentTarget.TakeDamage(Damage, CurrentPosition, DamageTriggers);
        }
    }
}