using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Lotl.Generic.Variables;
using Lotl.Units.Generic.StateMachine;
using Lotl.Units.Locomotion;

namespace Lotl.Units.Attackers
{
    [RequireComponent(typeof(MobileUnitLocomotion))]
    public class MobileSummoner : StaticSummoner, IMobileSeeker
    {
        public event Action OnStartMoving;
        public event Action OnEndMoving;

        [Header("Seek Data")]
        [SerializeField] private FloatReference seekRange;
        [SerializeField] private UnitTribeMask scanTribeMask;
        [SerializeField] private LayerMask scanMask;

        [Header("Mobile Seek Data")]
        [SerializeField] private FloatReference reach;
        [SerializeField] private MobileUnitLocomotion locomotion;

        private Unit currentTarget;

        public float Reach => reach;
        public MobileUnitLocomotion Locomotion => locomotion;

        public float SeekRange => seekRange;
        public UnitTribeMask ScanTribeMask => scanTribeMask;
        public LayerMask ScanMask => scanMask;

        public Unit CurrentTarget
        {
            get => currentTarget;
            set => currentTarget = value;
        }

        public Vector3 CurrentPosition => transform.position;

        protected override void Awake()
        {
            locomotion = GetComponent<MobileUnitLocomotion>();

            base.Awake();
        }

        public void BeganMoving()
        {
            OnStartMoving?.Invoke();
        }

        public void StoppedMoving()
        {
            OnEndMoving?.Invoke();
        }

#if UNITY_EDITOR

        protected virtual void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, SeekRange);
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, Reach);
        }

#endif
    }
}