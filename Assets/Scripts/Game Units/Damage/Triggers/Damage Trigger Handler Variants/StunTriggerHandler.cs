using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Lotl.StateMachine;
using Lotl.Generic.Variables;
using Lotl.Units.Locomotion;

namespace Lotl.Units.Damage
{
    [RequireComponent(typeof(Driver), typeof(MobileUnitLocomotion))]
    public class StunTriggerHandler : DamageTriggerHandler
    {
        [SerializeField] private Identity stunDuration;
        private bool isStunned = false;
        
        private Driver driver;
        private MobileUnitLocomotion locomotion;

        private void Start()
        {
            driver = GetComponent<Driver>();
            locomotion = GetComponent<MobileUnitLocomotion>();
        }

        protected override void RespondToTrigger(DamageInfo damageInfo, DamageTrigger trigger)
        {
            if (isStunned) return;
            float stunDuration = trigger.GetValue<float>(this.stunDuration);
            StartCoroutine(PauseDriver(stunDuration));
        }

        IEnumerator PauseDriver(float stunDuration)
        {
            isStunned = true;
            driver.Pause();
            locomotion.StopMoving();

            yield return new WaitForSeconds(stunDuration);
            
            isStunned = false;
            driver.Unpause();
            
        }
    }
}