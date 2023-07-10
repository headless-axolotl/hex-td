using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Lotl.Runtime;
using Lotl.Runtime.Generic;
using Lotl.StateMachine;
using Lotl.Generic.Variables;

namespace Lotl.Units.Towers
{
    [RequireComponent(typeof(Tower), typeof(Timer))]
    public class ProjectileClassTower : Driver
    {
        #region Properties

        [Header("Data")]
        [SerializeField] private Pool projectilePool;
        [SerializeField] private FloatReference range;
        [SerializeField] private UnitTribeMask tribeMask;
        [SerializeField] private LayerMask scanMask;
        [SerializeField] private Timer timer;
        [SerializeField] private Transform projectileSource;

        [Header("Runtime")]
        [SerializeField] private Unit currentTarget;
        
        public Pool ProjectilePool => projectilePool;
        public float Range => range;
        public UnitTribeMask TribeMask => tribeMask;
        public LayerMask ScanMask => scanMask;
        public Timer Timer => timer;
        public Transform ProjectileSource => projectileSource;
        public Unit CurrentTarget { get => currentTarget; set { currentTarget = value; } }

        #endregion

        protected override void Awake()
        {
            if (projectilePool == null)
                Debug.LogError($"ProjectileClassTower [{name}] is missing a projectile pool!");
            if (projectileSource == null)
                Debug.LogError($"ProjectileClassTower [{name}] is missing a projectile source!");
            timer = GetComponent<Timer>();
            
            base.Awake();
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, Range);
        }
    }
}
