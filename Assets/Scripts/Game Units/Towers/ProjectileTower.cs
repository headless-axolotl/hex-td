using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Lotl.Runtime;
using Lotl.Runtime.Generic;
using Lotl.StateMachine;
using Lotl.Generic.Variables;
using Lotl.Units.Generic.StateMachine;

namespace Lotl.Units.Towers
{
    [RequireComponent(typeof(AutounitSetAdder), typeof(Timer))]
    public class ProjectileTower : Driver, ISeeker
    {
        #region Events

        public event Action OnShootAction;

        public void TriggerShootAction()
            => OnShootAction?.Invoke();

        #endregion

        #region Properties

        [Header("Seek Data")]
        [SerializeField] private FloatReference seekRange;
        [SerializeField] private UnitTribeMask scanTribeMask;
        [SerializeField] private LayerMask scanMask;
        
        [Header("Action Data")]
        [SerializeField] private UnitTribeMask hitTribeMask;
        [SerializeField] private Pool projectilePool;
        [SerializeField] private Transform projectileSource;

        [Header("Timing")]
        [SerializeField] private Timer timer;
        [SerializeField] private FloatReference actionCooldown;
        [SerializeField] private FloatReference actionDelay;
        [SerializeField] private ActionState state = ActionState.Cooldown;
        public enum ActionState { Cooldown, Delay }

        [Header("Runtime")]
        [SerializeField] private Unit currentTarget;
        
        public float SeekRange => seekRange;
        public UnitTribeMask ScanTribeMask => scanTribeMask;
        public LayerMask ScanMask => scanMask;
        
        public UnitTribeMask HitTribeMask => hitTribeMask;
        public Pool ProjectilePool => projectilePool;
        public Transform ProjectileSource => projectileSource;
        
        public Timer Timer => timer;
        public float ActionCooldown => actionCooldown;
        public float ActionDelay => actionDelay;
        public ActionState State
        {
            get => state;
            set => state = value;
        }

        public Unit CurrentTarget
        {
            get => currentTarget;
            set => currentTarget = value;
        }

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

#if UNITY_EDITOR

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, SeekRange);
        }

#endif
    }
}
