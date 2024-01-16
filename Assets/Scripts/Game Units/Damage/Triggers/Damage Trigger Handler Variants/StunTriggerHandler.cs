using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Lotl.StateMachine;
using Lotl.Generic.Variables;
using Lotl.Units.Locomotion;

namespace Lotl.Units.Damage
{
    [RequireComponent(typeof(Driver), typeof(UnitLocomotion))]
    public class StunTriggerHandler : DamageTriggerHandler
    {
        [SerializeField] private FloatReference stunDuration;

        private bool isStunned = false;
        
        private Driver driver;
        private UnitLocomotion locomotion;

        private void Start()
        {
            driver = GetComponent<Driver>();
            locomotion = GetComponent<UnitLocomotion>();
        }

        protected override void RespondToTrigger(DamageInfo damageInfo)
        {
            if (isStunned) return;
            StartCoroutine(PauseDriver());
        }

        IEnumerator PauseDriver()
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