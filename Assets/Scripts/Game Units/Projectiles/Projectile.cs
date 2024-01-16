using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Lotl.Runtime.Generic;
using Lotl.Generic.Variables;
using Lotl.Units.Damage;

namespace Lotl.Units.Projectiles
{
    [RequireComponent(typeof(Timer))]
    public class Projectile : MonoBehaviour
    {
        #region Properties

        [Header("Data")]
        [SerializeField] protected FloatReference damage;
        [SerializeField] protected DamageTrigger[] damageTriggers;
        [SerializeField] protected FloatReference speed;
        [SerializeField] protected FloatReference hitRadius;
        [SerializeField] protected FloatReference lifetime;

        [SerializeField] protected LayerMask scanLayer;
        [SerializeField] protected UnitTribeMask scanTribeMask;
        [SerializeField] protected UnitTribeMask hitTribeMask;

        [Header("Runtime")]
        [SerializeField] protected Timer timer;

        #endregion

        #region Methods

        private void Awake()
        {
            timer = GetComponent<Timer>();
        }

        protected virtual void FixedUpdate()
        {
            Move();
            HitCheck();
        }

        private void OnEnable()
        {
            timer.Trigger(lifetime);
            timer.Done += LifetimeEnded;
        }

        private void OnDisable()
        {
            timer.Stop();
            timer.Done -= LifetimeEnded;
        }

        private void LifetimeEnded()
        {
            gameObject.SetActive(false);
        }

        public virtual void Initialize(ProjectileInfo projetileInfo)
        {
            transform.position = projetileInfo.source;
            AlignTo(projetileInfo.target);
            scanTribeMask = projetileInfo.scanTribeMask;
            hitTribeMask = projetileInfo.hitTribeMask;
        }

        protected virtual void AlignTo(Vector3 target)
        {
            transform.forward = (target - transform.position).normalized;
        }

        protected virtual void Move()
        {
            transform.position += (speed * Time.fixedDeltaTime * transform.forward);
        }

        protected virtual void HitCheck()
        {
            List<Unit> hitUnits = Scanner.Scan(
                transform.position, hitRadius,
                scanLayer, hitTribeMask);
            if (hitUnits.Count == 0) return;
            DealDamage(hitUnits);
        }

        protected virtual void DealDamage(List<Unit> units)
        {
            Unit targetUnit = units.FirstOrDefault();
            if (targetUnit != null) DealDamage(targetUnit);
            gameObject.SetActive(false);
        }

        protected virtual void DealDamage(Unit unit)
        {
            unit.TakeDamage(damage, transform.position, damageTriggers);
        }

        #endregion

#if UNITY_EDITOR

        protected virtual void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, hitRadius);
        }

#endif
    }
}
