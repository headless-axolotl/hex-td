using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Lotl.Runtime;
using Lotl.Runtime.Generic;
using Lotl.StateMachine;
using Lotl.Generic.Variables;
using Lotl.Units.Generic.StateMachine;
using Lotl.Units.Damage;

namespace Lotl.Units.Attackers
{
    [RequireComponent(typeof(Timer))]
    public class StaticRangedAttacker : Driver, IRangedAttacker
    {
        public event Action OnShootAction;

        [Header("Seek Data")]
        [SerializeField] protected FloatReference seekRange;
        [SerializeField] protected UnitTribeMask scanTribeMask;
        [SerializeField] protected LayerMask scanMask;
        
        [Header("Action Data")]
        [SerializeField] protected UnitTribeMask hitTribeMask;
        [SerializeField] protected Pool projectilePool;
        [SerializeField] protected Transform projectileSource;
        [SerializeField] protected DamageTrigger[] damageTriggers;
        [SerializeField] protected IdentityDictionary identityDictionary;

        [Header("Timing")]
        [SerializeField] protected Timer timer;
        [SerializeField] protected FloatReference actionCooldown;
        [SerializeField] protected FloatReference actionDelay;
        [SerializeField] protected ActionState state = ActionState.Cooldown;

        [Header("Runtime")]
        [SerializeField] protected Unit currentTarget;
        
        public float SeekRange => seekRange;
        public UnitTribeMask ScanTribeMask => scanTribeMask;
        public LayerMask ScanMask => scanMask;
        public Vector3 CurrentPosition => transform.position;

        public virtual float AttackRange => SeekRange;
        public UnitTribeMask HitTribeMask => hitTribeMask;
        public Pool ProjectilePool => projectilePool;
        public Vector3 ProjectileSource => projectileSource.transform.position;
        public DamageTrigger[] DamageTriggers => damageTriggers;
        public IdentityDictionary IdentityDictionary => identityDictionary;

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
                Debug.LogError($"{nameof(StaticRangedAttacker)} [{name}] is missing a projectile pool!");
            if (projectileSource == null)
                Debug.LogError($"{nameof(StaticRangedAttacker)} [{name}] is missing a projectile source!");
            timer = GetComponent<Timer>();
            
            base.Awake();
        }

        public void TriggerAttackEvent()
        {
            OnShootAction?.Invoke();
        }

#if UNITY_EDITOR

        protected virtual void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, AttackRange);
        }

#endif
    }
}
