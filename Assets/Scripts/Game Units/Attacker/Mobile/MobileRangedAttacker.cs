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

namespace Lotl.Units.Attackers
{
    [RequireComponent(typeof(Timer), typeof(MobileUnitLocomotion))]
    public class MobileRangedAttacker : StaticRangedAttacker, IMobileSeeker
    {
        public event Action OnStartMoving;
        public event Action OnEndMoving;

        [Header("Mobility Data")]
        [SerializeField] private FloatReference attackRange;
        [SerializeField] private MobileUnitLocomotion locomotion;

        public override float AttackRange => attackRange;
        
        public float Reach => attackRange;
        public MobileUnitLocomotion Locomotion => locomotion;


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

        protected override void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, SeekRange);
            base.OnDrawGizmosSelected();
        }

#endif
    }
}
