using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Lotl.StateMachine;

namespace Lotl.Gameplay.Gameloop.States
{
    [CreateAssetMenu(fileName = "BuildPhase", menuName = "Lotl/Gameplay/Gameloop/States/Build Phase")]
    public class BuildPhase : State
    {
        public override void OnEnter(Driver driver)
        {
            if (driver is not GameplayManager gameManager)
            {
                Debug.LogError($"Driver [{driver.name}] entered incompatible State [{name}]!");
                return;
            }
            gameManager.ReadyFlag = false;
            gameManager.SaveState();
        }

        public override void Tick(Driver driver)
        {

        }

        public override void OnExit(Driver driver)
        {
            if (driver is not GameplayManager gameManager) return;
            gameManager.SaveState();
        }
    }
}
