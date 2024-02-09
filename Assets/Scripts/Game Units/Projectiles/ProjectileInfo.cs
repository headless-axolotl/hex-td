using Lotl.Generic.Variables;
using Lotl.Units.Damage;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lotl.Units.Projectiles
{
    public struct ProjectileInfo
    {
        public Vector3 source;
        public Vector3 target;
        public UnitTribeMask scanTribeMask;
        public UnitTribeMask hitTribeMask;
        public DamageTrigger[] damageTriggers;
        public IdentityDictionary identityDictionary;

        public ProjectileInfo(
            Vector3 source,
            Vector3 target,
            UnitTribeMask scanTribeMask,
            UnitTribeMask hitTribeMask,
            DamageTrigger[] damageTriggers,
            IdentityDictionary identityDictionary)
        {
            this.source = source;
            this.target = target;
            this.scanTribeMask = scanTribeMask;
            this.hitTribeMask = hitTribeMask;
            this.damageTriggers = damageTriggers;
            this.identityDictionary = identityDictionary;
        }
    }
}
