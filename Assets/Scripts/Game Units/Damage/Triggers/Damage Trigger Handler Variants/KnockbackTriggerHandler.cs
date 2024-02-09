using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Lotl.Generic.Variables;

namespace Lotl.Units.Damage
{
    [RequireComponent(typeof(Rigidbody))]
    public class KnockbackTriggerHandler : DamageTriggerHandler
    {
        [SerializeField] private Identity explosionForceId;
        [SerializeField] private Identity explosionRadiusId;

        private Rigidbody rb;

        private void Start()
        {
            rb = GetComponent<Rigidbody>();
        }

        protected override void RespondToTrigger(DamageInfo damageInfo, DamageTrigger trigger)
        {
            float explosionForce = trigger.Dictionary.GetValue<float>(explosionForceId);
            float explosionRadius = trigger.Dictionary.GetValue<float>(explosionRadiusId);
            rb.AddExplosionForce(explosionForce * damageInfo.Amount, damageInfo.Source, explosionRadius);
        }
    }
}