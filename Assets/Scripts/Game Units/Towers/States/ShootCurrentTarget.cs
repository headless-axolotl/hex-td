using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Lotl.StateMachine;
using Lotl.Units.Projectiles;

namespace Lotl.Units.Towers.States
{
    [CreateAssetMenu(fileName = "ShootCurrentTarget", menuName = "Units/Towers/Projectile Class/States/Shoot Current Target")]
    public class ShootCurrentTarget : State
    {
        public override void OnEnter(Driver driver)
        {
            if (driver is not ProjectileClassTower)
            {
                Debug.LogError($"Driver [{driver.name}] entered incompatible State [{name}]!");
                return;
            }
        }

        public override void Tick(Driver driver)
        {
            if (driver is not ProjectileClassTower tower) return;
            
            if (!IsCurrentTargetValid(tower)) return;

            if (tower.Timer.IsTicking) return;
            tower.Timer.Trigger();

            if (!tower.ProjectilePool.GetObject().TryGetComponent<Projectile>(out var projectile))
            {
                Debug.LogError("Tower projectile pool returned an object without " +
                    "a Projectile component!");
                return;
            }

            projectile.Initialize(CreateProjectileInfo(tower));
            projectile.gameObject.SetActive(true);
        }

        private bool IsCurrentTargetValid(ProjectileClassTower tower)
        {
            if (tower.CurrentTarget == null) return false;
            float distance = (tower.transform.position - tower.CurrentTarget.transform.position).magnitude;
            if (distance > tower.Range)
            {
                tower.CurrentTarget = null;
                return false;
            }
            return true;
        }

        private ProjectileInfo CreateProjectileInfo(ProjectileClassTower tower)
        {
            return new(
                tower.ProjectileSource.position,
                tower.CurrentTarget.transform.position,
                tower.TribeMask);
        }

        public override void OnExit(Driver driver) { }
    }
}
