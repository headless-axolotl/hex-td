using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Lotl.StateMachine;
using Lotl.Generic.Variables;
using Lotl.Runtime.Generic;
using Lotl.Units.Generic.StateMachine;
using Lotl.Units.Locomotion;

namespace Lotl.Units.Enemies
{
    [RequireComponent(typeof(Timer), typeof(UnitLocomotion))]
    public class MeleeEnemy : Driver, ISeeker
    {
        #region Events

        public event Action OnAttackAction;

        public void TriggerAttackAction()
            => OnAttackAction?.Invoke();

        #endregion

        #region Properties

        [Header("Seek Data")]
        [SerializeField] private FloatReference seekRange;
        [SerializeField] private UnitTribeMask scanTribeMask;
        [SerializeField] private LayerMask scanMask;
        
        [Header("Action Data")]
        [SerializeField] private FloatReference meleeRange;
        [SerializeField] private FloatReference attackPower;
        [SerializeField] private UnitLocomotion locomotion;

        [Header("Timing")]
        [SerializeField] private Timer timer;
        [SerializeField] private FloatReference actionCooldown;
        [SerializeField] private FloatReference actionDelay;
        [SerializeField] private ActionState state;

        public enum ActionState { Cooldown, Delay }

        [Header("Runtime")]
        [SerializeField] private Unit currentTarget;

        public float SeekRange => seekRange;
        public UnitTribeMask ScanTribeMask => scanTribeMask;
        public LayerMask ScanMask => scanMask;
        
        public float MeleeRange => meleeRange;
        public float AttackPower => attackPower;
        public UnitLocomotion Locomotion => locomotion;
        
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

            base.Awake();
        }

#if UNITY_EDITOR

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, SeekRange);
            Gizmos.color = Color.black;
            Gizmos.DrawSphere(transform.position + MeleeRange * transform.forward, 0.2f);
        }

#endif
    }
}
