using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Lotl.StateMachine;
using Lotl.Units.Projectiles;

namespace Lotl.Units.Towers.States
{
    [CreateAssetMenu(fileName = "ShootCurrentTarget", menuName = "Lotl/Units/Towers/Projectile Class/States/Shoot Current Target")]
    public class ShootCurrentTarget : State
    {
        public override void OnEnter(Driver driver)
        {
            if (driver is not ProjectileTower)
            {
                Debug.LogError($"Driver [{driver.name}] entered incompatible State [{name}]!");
                return;
            }
        }

        public override void Tick(Driver driver)
        {
            if (driver is not ProjectileTower tower) return;
            
            if (!IsCurrentTargetValid(tower)) return;

            if (tower.Timer.IsTicking) return;
            if (tower.State == ProjectileTower.ActionState.Cooldown)
            {
                tower.Timer.Trigger(tower.ActionDelay);
                tower.State = ProjectileTower.ActionState.Delay;
                tower.TriggerShootAction();
                return;
            }
            else
            {
                tower.Timer.Trigger(tower.ActionCooldown);
                tower.State = ProjectileTower.ActionState.Cooldown;
            }

            if (!tower.ProjectilePool.GetObject().TryGetComponent<Projectile>(out var projectile))
            {
                Debug.LogError("Tower projectile pool returned an object without " +
                    "a Projectile component!");
                return;
            }

            projectile.Initialize(CreateProjectileInfo(tower));
            projectile.gameObject.SetActive(true);
        }

        private bool IsCurrentTargetValid(ProjectileTower tower)
        {
            if (tower.CurrentTarget == null) return false;
            float sqrDistance = (tower.transform.position - tower.CurrentTarget.transform.position).sqrMagnitude;
            float range = tower.SeekRange;
            if (sqrDistance > range * range)
            {
                tower.CurrentTarget = null;
                return false;
            }
            return true;
        }

        private ProjectileInfo CreateProjectileInfo(ProjectileTower tower)
        {
            return new(
                tower.ProjectileSource.position,
                tower.CurrentTarget.transform.position,
                tower.ScanTribeMask,
                tower.HitTribeMask);
        }

        public override void OnExit(Driver driver) { }
    }
}
