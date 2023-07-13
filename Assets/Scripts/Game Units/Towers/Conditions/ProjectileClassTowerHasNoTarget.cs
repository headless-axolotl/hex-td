using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Lotl.StateMachine;

namespace Lotl.Units.Towers.Conditions
{
    [CreateAssetMenu(fileName = "HasNoTarget", menuName = "Lotl/Units/Towers/Projectile Class/Conditions/Has No Target")]
    public class ProjectileClassTowerHasNoTarget : Condition
    {
        public override bool IsMet(Driver driver)
        {
            if (driver is not ProjectileClassTower tower)
            {
                Debug.LogError($"Driver [{driver.name}] used incpomatible Condition [{name}]!");
                return false;
            }
            return tower.CurrentTarget == null;
        }
    }
}
