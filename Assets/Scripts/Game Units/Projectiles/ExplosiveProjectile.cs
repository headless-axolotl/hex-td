using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Lotl.Generic.Variables;

namespace Lotl.Units.Projectiles
{
    public class ExplosiveProjectile : Projectile
    {
        [Header("Additional Data")]
        [SerializeField] private Identity damageRadiusId;
        private float damageRadius = 0f;

        public override void Initialize(ProjectileInfo projectileInfo)
        {
            base.Initialize(projectileInfo);

            damageRadius = currentConfiguration.GetValue<float>(damageRadiusId);
        }

        protected override void HitCheck()
        {
            List<Unit> hitUnits = Scanner.Scan(
                transform.position, hitRadius,
                scanLayer, hitTribeMask);

            if (hitUnits.Count == 0) return;

            Explode();
        }

        private void Explode()
        {
            List<Unit> hitUnitsFromExplosion = Scanner.Scan(
                transform.position, damageRadius,
                scanLayer, scanTribeMask);
            DealDamage(hitUnitsFromExplosion);
        }

        protected override void DealDamage(List<Unit> units)
        {
            foreach(Unit unit in units)
            {
                DealDamage(unit);
            }
            gameObject.SetActive(false);
        }
    }
}