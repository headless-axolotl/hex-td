using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Lotl.StateMachine;

namespace Lotl.Gameplay.Gameloop.States
{
    [CreateAssetMenu(fileName = "WavePhase", menuName = "Lotl/Gameplay/Gameloop/States/Wave Phase")]
    public class WavePhase : State
    {
        public override void OnEnter(Driver driver)
        {
            if (driver is not GameplayManager gameManager)
            {
                Debug.LogError($"Driver [{driver.name}] entered incompatible State [{name}]!");
                return;
            }
            gameManager.ReadyFlag = false;
        }
        
        public override void Tick(Driver driver)
        {
            
        }

        public override void OnExit(Driver driver)
        {
            
        }
    }
}
