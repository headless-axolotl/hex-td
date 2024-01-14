using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Lotl.StateMachine;
using Lotl.Generic.Variables;
using Lotl.Units.Enemies;

namespace Lotl.Units.Damage
{
    [RequireComponent(typeof(Driver), typeof(EnemyLocomotion))]
    public class StunTriggerHandler : DamageTriggerHandler
    {
        [SerializeField] private FloatReference stunDuration;

        private bool isStunned = false;
        
        private Driver driver;
        private EnemyLocomotion locomotion;

        private void Start()
        {
            driver = GetComponent<Driver>();
            locomotion = GetComponent<EnemyLocomotion>();
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