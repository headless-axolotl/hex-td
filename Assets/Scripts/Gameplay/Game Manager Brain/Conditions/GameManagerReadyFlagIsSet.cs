using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Lotl.StateMachine;
using Lotl.Units.Towers;

namespace Lotl.Gameplay.Gameloop.Conditions
{
    [CreateAssetMenu(fileName = "ReadyFlagIsSet", menuName = "Lotl/Gameplay/Gameloop/Conditions/Ready Flag Is Set")]
    public class GameManagerReadyFlagIsSet : Condition
    {
        public override bool IsMet(Driver driver)
        {
            if (driver is not GameManager gameManager)
            {
                Debug.LogError($"Driver [{driver.name}] used incpomatible Condition [{name}]!");
                return false;
            }
            return gameManager.ReadyFlag;
        }
    }
}
