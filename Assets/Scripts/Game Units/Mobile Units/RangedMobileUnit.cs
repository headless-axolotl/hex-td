using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Lotl.StateMachine;
using Lotl.Units.Generic.StateMachine;
using Lotl.Runtime;
using Lotl.Runtime.Generic;
using Lotl.Units.Locomotion;
using Lotl.Generic.Variables;

namespace Lotl.Units.Enemies
{
    [RequireComponent(typeof(Timer), typeof(MobileUnitLocomotion))]
    public class RangedMobileUnit : Driver, IRangedAttacker, IMobileSeeker
    {
        public event Action OnShootAction;

        [Header("Seek Data")]
        [SerializeField] private FloatReference seekRange;
        [SerializeField] private UnitTribeMask scanTribeMask;
        [SerializeField] private LayerMask scanMask;
        [SerializeField] private MobileUnitLocomotion locomotion;

        [Header("Action Data")]
        [SerializeField] private FloatReference attackRange;
        [SerializeField] private UnitTribeMask hitTribeMask;
        [SerializeField] private Pool projectilePool;
        [SerializeField] private Transform projectileSource;

        [Header("Timing")]
        [SerializeField] private Timer timer;
        [SerializeField] private FloatReference actionCooldown;
        [SerializeField] private FloatReference actionDelay;
        [SerializeField] private ActionState state = ActionState.Cooldown;

        [Header("Runtime")]
        [SerializeField] private Unit currentTarget;

        public float SeekRange => seekRange;
        public UnitTribeMask ScanTribeMask => scanTribeMask;
        public LayerMask ScanMask => scanMask;
        public Vector3 CurrentPosition => transform.position;

        public float AttackRange => attackRange;
        public UnitTribeMask HitTribeMask => hitTribeMask;
        public Pool ProjectilePool => projectilePool;
        public Vector3 ProjectileSource => projectileSource.transform.position;

        public float Reach => attackRange;
        public MobileUnitLocomotion Locomotion => locomotion;

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

        protected override void Awake()
        {
            if (projectilePool == null)
                Debug.LogError($"{nameof(RangedMobileUnit)} [{name}] is missing a projectile pool!");
            if (projectileSource == null)
                Debug.LogError($"{nameof(RangedMobileUnit)} [{name}] is missing a projectile source!");
            timer = GetComponent<Timer>();
            locomotion = GetComponent<MobileUnitLocomotion>();

            base.Awake();
        }

        public void TriggerAttackEvent()
        {
            OnShootAction?.Invoke();
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
