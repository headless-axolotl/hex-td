using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Lotl.Units.Projectiles;
using Lotl.Runtime.Generic;
using Lotl.Runtime;
using Lotl.Units.Damage;
using Lotl.Generic.Variables;

namespace Lotl.Units.Generic.StateMachine
{
    public interface IRangedAttacker : IAttacker
    {
        UnitTribeMask HitTribeMask { get; }
        Pool ProjectilePool { get; }
        Vector3 ProjectileSource { get; }
        DamageTrigger[] DamageTriggers { get; }
        IdentityDictionary IdentityDictionary { get; }

        void Attack()
        {
            if (!ProjectilePool.GetObject().TryGetComponent<Projectile>(out var projectile))
            {
                Debug.LogError("Tower projectile pool returned an object without " +
                    "a Projectile component!");
                return;
            }

            projectile.Initialize(CreateProjectileInfo());
            projectile.gameObject.SetActive(true);
        }

        ProjectileInfo CreateProjectileInfo()
        {
            return new(
                ProjectileSource,
                CurrentTarget.transform.position,
                ScanTribeMask,
                HitTribeMask,
                DamageTriggers,
                IdentityDictionary);
        }
    }
}
