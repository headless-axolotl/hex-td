using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Lotl.Generic.Variables;

namespace Lotl.Units.Damage
{
    [RequireComponent(typeof(Rigidbody))]
    public class KnockbackTriggerHandler : DamageTriggerHandler
    {
        [SerializeField] private FloatReference explosionForce;
        [SerializeField] private FloatReference explosionRadius;

        private Rigidbody rb;

        private void Start()
        {
            rb = GetComponent<Rigidbody>();
        }

        protected override void RespondToTrigger(DamageInfo damageInfo)
        {
            rb.AddExplosionForce(explosionForce * damageInfo.Amount, damageInfo.Source, explosionRadius);
        }
    }
}