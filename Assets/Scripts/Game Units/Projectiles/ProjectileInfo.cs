using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lotl.Units.Projectiles
{
    public struct ProjectileInfo
    {
        public Vector3 source;
        public Vector3 target;
        public UnitTribeMask mask;

        public ProjectileInfo(Vector3 source, Vector3 target, UnitTribeMask mask)
        {
            this.source = source;
            this.mask = mask;
            this.target = target;
        }
    }
}
