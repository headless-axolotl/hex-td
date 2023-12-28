using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Lotl.Runtime.Generic;
using Lotl.Generic.Variables;
using System.Linq;
using System;

namespace Lotl.Units.Projectiles
{
    [RequireComponent(typeof(Timer))]
    public class Projectile : MonoBehaviour
    {
        #region Properties

        [Header("Data")]
        [SerializeField] protected FloatReference damage;
        [SerializeField] protected FloatReference speed;
        [SerializeField] protected FloatReference hitRadius;
        [SerializeField] protected FloatReference lifetime;

        [SerializeField] protected LayerMask scanLayer;
        [SerializeField] protected UnitTribeMask unitMask;

        [Header("Runtime")]
        [SerializeField] protected Timer timer;

        #endregion

        #region Methods

        private void Awake()
        {
            timer = GetComponent<Timer>();
            timer.Done += LifetimeEnded;
        }

        protected virtual void FixedUpdate()
        {
            Move();
            HitCheck();
        }

        private void OnEnable()
        {
            timer.Trigger(lifetime);
        }

        private void OnDisable()
        {
            timer.Stop();
        }

        private void LifetimeEnded()
        {
            gameObject.SetActive(false);
        }

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
