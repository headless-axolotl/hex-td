using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Lotl.StateMachine;

namespace Lotl.Units.Enemies.States
{
    [CreateAssetMenu(fileName = "AttackCurrentTarget", menuName = "Lotl/Units/Enemies/Melee/States/Attack Current Target")]
    public class AttackCurrentTarget : State
    {
        public override void OnEnter(Driver driver)
        {
            if (driver is not MeleeEnemy enemy)
            {
                Debug.LogError($"Driver [{driver.name}] entered incompatible State [{name}]!");
                return;
            }
            enemy.Locomotion.StopMoving();
        }

        public override void Tick(Driver driver)
        {
            if (driver is not MeleeEnemy enemy) return;

            if (enemy.CurrentTarget == null) return;
            
            if (ShouldGetCloserToTower(enemy))
            {
                enemy.Locomotion.MoveTowards(enemy.CurrentTarget.transform.position);
                return;
            }
            else enemy.Locomotion.StopMoving();
            
            if (enemy.Timer.IsTicking) return;
            if (enemy.State == MeleeEnemy.ActionState.Cooldown)
            {
                enemy.Timer.Trigger(enemy.ActionDelay);
                enemy.State = MeleeEnemy.ActionState.Delay;
                enemy.TriggerAttackAction();
                return;
            }
            else
            {
                enemy.Timer.Trigger(enemy.ActionCooldown);
                enemy.State = MeleeEnemy.ActionState.Cooldown;
            }

            enemy.CurrentTarget.TakeDamage(enemy.AttackPower);
        }

        public bool ShouldGetCloserToTower(MeleeEnemy enemy)
        {
            float sqrDistance = (enemy.transform.position - enemy.CurrentTarget.transform.position).sqrMagnitude;
            float range = enemy.MeleeRange;
            if (sqrDistance >  range * range) return true;
            return false;
        }

        public override void OnExit(Driver driver)
        {
            if (driver is not MeleeEnemy enemy) return;
            enemy.Locomotion.StopMoving();
        }
    }
}
