using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Lotl.StateMachine;
using Lotl.Generic.Variables;
using Lotl.Runtime.Generic;
using Lotl.Units.Generic.StateMachine;
using Lotl.Units.Locomotion;
using Lotl.Units.Damage;

namespace Lotl.Units.Enemies
{
    [RequireComponent(typeof(Timer), typeof(MobileUnitLocomotion))]
    public class MeleeMobileUnit : Driver, IMeleeAttacker, IMobileSeeker
    {
        public event Action OnAttackAction;

        #region Properties

        [Header("Seek Data")]
        [SerializeField] private FloatReference seekRange;
        [SerializeField] private UnitTribeMask scanTribeMask;
        [SerializeField] private LayerMask scanMask;
        
        [Header("Action Data")]
        [SerializeField] private FloatReference attackRange;
        [SerializeField] private FloatReference damage;
        [SerializeField] private MobileUnitLocomotion locomotion;

        [Header("Timing")]
        [SerializeField] private Timer timer;
        [SerializeField] private FloatReference actionCooldown;
        [SerializeField] private FloatReference actionDelay;
        [SerializeField] private ActionState state;

        [Header("Runtime")]
        [SerializeField] private Unit currentTarget;

        public float SeekRange => seekRange;
        public UnitTribeMask ScanTribeMask => scanTribeMask;
        public LayerMask ScanMask => scanMask;
        public Vector3 CurrentPosition => transform.position;
        
        public float AttackRange => attackRange;
        public float Damage => damage;
        public DamageTrigger[] DamageTriggers => null;

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

        #endregion

        protected override void Awake()
        {
            timer = GetComponent<Timer>();
            locomotion = GetComponent<MobileUnitLocomotion>();

            base.Awake();
        }

        public void TriggerAttackEvent()
        {
            OnAttackAction?.Invoke();
        }

#if UNITY_EDITOR

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, SeekRange);
            Gizmos.color = Color.black;
            Gizmos.DrawSphere(transform.position + AttackRange * transform.forward, 0.2f);
        }

#endif
    }
}
