using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Lotl.Generic.Variables;
using System.Linq;

namespace Lotl.Units.Projectiles
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] protected FloatReference damage;
        [SerializeField] protected FloatReference speed;
        [SerializeField] protected FloatReference hitRadius;
        
        [SerializeField] protected LayerMask scanLayer;
        [SerializeField] protected UnitTribeMask unitMask;

        public virtual void Initialize(ProjectileInfo projetileInfo)
        {
            transform.position = projetileInfo.source;
            transform.forward = (projetileInfo.target - transform.position).normalized;
            unitMask = projetileInfo.mask;
        }

        protected virtual void Move()
        {
            transform.position += (speed * Time.fixedDeltaTime * transform.forward);
        }

        protected virtual void HitCheck()
        {
            List<Unit> hitUnits = Utility.Scan(transform.position, hitRadius, scanLayer, unitMask);
            if (hitUnits.Count == 0) return;
            DealDamage(hitUnits);
        }

        protected virtual void DealDamage(List<Unit> units)
        {
            Unit targetUnit = units.FirstOrDefault();
            if (targetUnit != null)
                targetUnit.TakeDamage(damage);
            gameObject.SetActive(false);
        }

        protected virtual void FixedUpdate()
        {
            Move();
            HitCheck();
        }
    }

}
