using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Lotl.Generic.Variables;

namespace Lotl.Units.Damage
{
    [RequireComponent(typeof(Rigidbody))]
    public class KnockbackTriggerHandler : DamageTriggerHandler
    {
        [SerializeField] private Identity explosionForce;
        [SerializeField] private Identity explosionRadius;

        private Rigidbody rb;

        private void Start()
        {
            rb = GetComponent<Rigidbody>();
        }

        protected override void RespondToTrigger(DamageInfo damageInfo, DamageTrigger trigger)
        {
            float explosionForce = trigger.GetValue<float>(this.explosionForce);
            float explosionRadius = trigger.GetValue<float>(this.explosionRadius);
            rb.AddExplosionForce(explosionForce * damageInfo.Amount, damageInfo.Source, explosionRadius);
        }
    }
}