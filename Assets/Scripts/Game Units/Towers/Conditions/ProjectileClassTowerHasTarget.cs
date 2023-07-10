using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Lotl.StateMachine;

namespace Lotl.Units.Towers.Conditions
{
    [CreateAssetMenu(fileName = "HasTarget", menuName = "Units/Towers/Projectile Class/Conditions/Has Target")]
    public class ProjectileClassTowerHasTarget : Condition
    {
        public override bool IsMet(Driver driver)
        {
            if (driver is not ProjectileClassTower tower)
            {
                Debug.LogError($"Driver [{driver.name}] used incpomatible Condition [{name}]!");
                return false;
            }
            return tower.CurrentTarget != null;
        }
    }
}
