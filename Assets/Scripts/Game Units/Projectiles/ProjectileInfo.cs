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

        public ProjectileInfo(
            Vector3 source,
            Vector3 target,
            UnitTribeMask scanTribeMask,
            UnitTribeMask hitTribeMask)
        {
            this.source = source;
            this.target = target;
            this.scanTribeMask = scanTribeMask;
            this.hitTribeMask = hitTribeMask;
        }
    }
}
